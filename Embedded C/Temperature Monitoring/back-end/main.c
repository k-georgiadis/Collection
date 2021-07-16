/*
MIT License
Copyright (c) 2021 Kosmas Georgiadis
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

/*
 * main.c
 *
 * Created: 9/13/2016 10:32:09 PM
 * Author : Kosmas Georgiadis
 */ 
 
#define F_CPU 16000000UL

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
//#include <inttypes.h>
#include <math.h>
#include <ctype.h>

#define BAUD_RATE 57600

#define SENT_STRING_SIZE 128
#define RESPONSE_STRING_SIZE 128
#define ESP_TRANSMIT_DELAY_MS 10

#define DEFAULT_ANSWER "OK"
#define DEFAULT_APPENDER "\r\n"
#define TIMEOUT_STRING "TIMED OUT"


//----------------Constants for specific NTC thermistor.-----------------------

#define REF_VOLT 3.3 //Reference voltag 3.3V

#define a 0.000946018574683 //Values after solving 3-equation system.
#define b 0.000242434141321
#define c 0.000000224104934229

#define R0  10000.0 //Resistance at Reference Temperature.

//-------------------------------------------------------------------------
/*	##################################################################################
	### size_of_buffer = 4^n ##### n = Number of desired extra bits in the resolution.
	##################################################################################
	We need 2 extra bits, therefore n = 2.
	10 + 2(extra) = 12 bits currently used. See Application Note AVR121 for more info.
	##################################################################################
*/
#define BUFFERSIZE 16 //4^2 = 16
#define ADC_PIN 4 //Set ADC pin for thermistor.

//-------------------------------------------------------------------------
static void InitESP8266();
static void init();
static void TimerInit();
static void ResetTimer();
//----------------------------------
static void InitADC();
//----------------------------------
static float CalculateTemperature();
static uint16_t StartADCReading();
static uint16_t ReadADC(uint8_t ch);
//----------------------------------
static void USARTInit(uint32_t ubrr_value);
static char USARTReadChar();
static void USARTWriteChar(char data, char slow);
static void USARTWriteString(char *string, char append, char slow);
//----------------------------------
uint8_t Send_StringUSART(char *string, uint8_t delay, char *stop_string, char appendOnCommand, char appendOnResponse, char slow);
static void Send_TimeOutString();
static void USART_FlushInputBuffer();
//----------------------------------
static void Listen_Connections();
static void CheckResponses(char *request);
//----------------------------------
static void Debug(char *string);
//----------------------------------
static void Send_TemperatureValue(char *conn_string);
//----------------------------------
static void Close_Connection(char *conn_string);
static uint16_t ParseUInt16(char *string);
static void Wait(uint8_t wait_seconds);
//----------------------------------
volatile uint8_t seconds = 0;
volatile uint8_t timeout = 0;
uint8_t delay = 0;
float Temperature = 0.0;	

void main()
{
	uint32_t UBR = F_CPU/BAUD_RATE - 16; //UBR Value Formula => F_CPU/(BAUD_RATE*16) - 1
	UBR = UBR/16;
	
	init();
	InitADC();
	TimerInit();
	USARTInit(UBR); 
	
	_delay_ms(10000);
	InitESP8266();

	while(1)
	{	
		//Listen for connections.
		Listen_Connections();
	}
}
//----------------------------------------------------------
//----------------------------------------------------------
void init()
{
	//--LED
	DDRB=(1<< PINB5);
	//---------------------
}
void InitADC()
{
	ADMUX|=(1<<REFS0);//refV=Vcc;
	ADCSRA=(1<<ADEN)|(1<<ADPS2)|(1<<ADPS1)|(1<<ADPS0); //Prescaler=128
}
void TimerInit()
{
	//Timer 2;
	//TIMSK |= (1 << TOIE0);
	//TCCR0 |= (1 << CS02);
	
	//Timer 1.
	TCCR1B = (1 << WGM12); // Mode 4, CTC on OCR1A
	OCR1A = 15624;
	TIMSK |= (1 << OCIE1A); //Set interrupt on compare match.
	
	sei();
}
void USARTInit(uint32_t ubrr_value)
{
   //Set Baud rate.
   UBRRL = ubrr_value;
   UBRRH = (ubrr_value>>8);

   /* Set Frame Format
   >> Asynchronous mode
   >> No Parity
   >> 1 StopBit

   >> char size 8   */
   UCSRC=(1<<URSEL)|(3<<UCSZ0);

   //Enable The receiver and transmitter.
   UCSRB=(1<<RXEN)|(1<<TXEN);
}
void InitESP8266()
{	
	//Send_StringUSART("AT+RST\r\n", 5, "OK\r\n"); // reset module.
	Send_StringUSART("ATE0", 1, DEFAULT_ANSWER, 1, 1, 0); // disable echo.
	Send_StringUSART("AT+CWMODE=3", 1, DEFAULT_ANSWER, 1, 1, 0); // configure as access point and client.
	Send_StringUSART("AT+CIPMUX=1", 1, DEFAULT_ANSWER, 1, 1, 0); // configure for multiple connections.
	Send_StringUSART("AT+CIFSR", 1, DEFAULT_ANSWER, 1, 1, 0); // get ip address.
	Send_StringUSART("AT+CIPSERVER=1,80", 1, DEFAULT_ANSWER, 1, 1, 0); // turn on server on port 80.
}
//----------------------------------------------------------
void ResetTimer()
{
	timeout = 0; //Reset timeout period.
	seconds = 0; //Reset seconds.
	TCCR1B |= (1 << CS12) | (1 << CS10); //Set prescaler to 1024 and start the timer.
	TCNT1 = 0;
}
//----------------------------------------------------------
char USARTReadChar()
{
	while(!(UCSRA & (1<<RXC)) && !timeout){} //Wait until timeout expires.
	
	if(timeout)
		return '\0';

	//Now USART has got data from host.
	return UDR;
}
void USARTWriteChar(char data, char slow)
{
   //Wait until the transmitter is ready.
   while(!(UCSRA & (1<<UDRE)))
   {
      //Do nothing
   }

   //Now write the data to USART buffer.
   UDR=data;
   
   if(slow)
		_delay_ms(ESP_TRANSMIT_DELAY_MS);
}
void USARTWriteString(char *string, char append, char slow)
{	
	char len = strlen(string); //Length of string, without null character.
	char counter = 0;
	char *head = string; //Save string pointer.
	
	while(*string)
	{
		/* 
			The following check was implemented due to the fact that when sending data to the ESP slowly,
			after the last character the ESP would send "SEND OK", but we would not receive it because we had the delay on USARTWriteChar.
			Therefore to catch up with the ESP, when we send the last character, we tell the USART writer to skip the delay.
		*/
		//If on last character, don't delay.
		if(counter == len - 1)
			USARTWriteChar(*string, 0);
		else
			USARTWriteChar(*string, slow);
			
		string++;
		counter++;
	}		
	string = head; //Restore string pointer.	
	
	//Append appender if said so.
	if(append)
	{
		char appender[3] = {0};
		strcpy(appender, DEFAULT_APPENDER);
		
		string = appender;
		while(*string)
		{
			USARTWriteChar(*string, 0);
			string++;
		}
	}
}
//----------------------------------------------------------
uint8_t Send_StringUSART(char *sent_string, uint8_t set_delay, char *response_string, char appendOnCommand, char appendOnResponse, char slow)
{	
	char response[RESPONSE_STRING_SIZE] = {0};  //Initialize response string.
	char string[SENT_STRING_SIZE] = {0}; //Initialize string to send.
	char stop_string[15] = {0}; //Initialize response string.
	char return_code = 1; //Return code.
    //Send_StringUSART(finalResult, 3, "SEND OK", 0, 0, 1);
	delay = set_delay; //Set delay.

	//Assign strings.
	strcpy(string, sent_string);
	strcpy(stop_string, response_string);

	//Appender on response string, if said so.
	if(appendOnResponse)
		strcat(stop_string, DEFAULT_APPENDER);

	USARTWriteString(string, appendOnCommand, slow); //Send string with append parameter.
	ResetTimer();

	uint8_t i = 0;
	do 
	{
		response[i] = USARTReadChar();
		if(strstr(response, stop_string))
			break;	
	}
	while (response[i++] && i < RESPONSE_STRING_SIZE - 1);

	TCCR1B &= ~((1 << CS12)|(1 << CS10)); //Stop timer.
	//response[i] = '\0';

	USART_FlushInputBuffer(); //Discard rest of input.

	if(timeout)
	{
		Send_TimeOutString(); //Send timeout string.
		return_code = 0; //Return timeout code.
	}
	timeout = 0; //Reset timeout flag.

	return return_code;
}
void Send_TimeOutString()
{
	char timeout_String[15] = {0}; //Init string.
	strcpy(timeout_String, TIMEOUT_STRING); //Assign timeout string, with null character.
	USARTWriteString(timeout_String, 1, 0); //Send timeout string.
}
void USART_FlushInputBuffer()
{
	//A bad implementation of this function can get you all sorts of troubles.
	//Debugging is hard as hell, in these types of situations.
	
	_delay_ms(1);
	while(UCSRA & (1<<RXC))
	{
		char temp = UDR;
		if(!temp) break;
		_delay_ms(1);
	}
}
void Listen_Connections()
{
	char conn_Req[RESPONSE_STRING_SIZE] = {0};
	uint8_t i = 0;
	uint8_t found = 0;

	timeout = 0;
	PORTB |= (1 << PINB5);
	do
	{
		conn_Req[i] = USARTReadChar();
		if(strstr(conn_Req, "Host:"))	
		{
			PORTB ^= (1 << PINB5);
			found = 1;
			break;
		}
	} 
	while (conn_Req[i++] && i < RESPONSE_STRING_SIZE - 1);

	conn_Req[i + found] = '\0';	
	//Debug(conn_Req);
	
	USART_FlushInputBuffer(); //Discard rest of input.
	
	//Check connection request, if found.
	if(found)
		CheckResponses(conn_Req);
}
//----------------------------------------------------------
void Debug(char *string)
{
	USARTWriteChar('-', 0);
	USARTWriteChar('-', 0);
	USARTWriteChar('-', 0);
	USARTWriteChar('\n', 0);
	USARTWriteString(string, 1, 0); //Send response to PC, for debugging.
	USARTWriteChar('-', 0);
	USARTWriteChar('-', 0);
	USARTWriteChar('-', 0);
	USARTWriteChar('\n', 0);
}
//----------------------------------------------------------
void CheckResponses(char *request)
{
	//If we are getting data.
	if(request)
	{
		//Get connection ID from ESP GET request: ----> +IPD,0,345:GET /?param=123 HTTP/1.1...
		char conn_string[5] = {0};	
		sprintf(conn_string, "%d", ParseUInt16(request)); 
		
		char *data = strstr(request, "?TEMP=1");
		
		if(data) //If user is requesting temperature value.
			Send_TemperatureValue(conn_string); //Send GET Request with temperature value to the specified connection.
			
		//Wait one (1) second before closing the connection, according to the manual.
		Wait(1);
		
		//Close connection.
		Close_Connection(conn_string);
	}
	else
		Send_StringUSART("AT", 1, DEFAULT_ANSWER, 1, 1, 0);
}
void Send_TemperatureValue(char *conn_string)
{
	char finalResult[SENT_STRING_SIZE] = {0};
	char tempValue[8] = {0}; 
	char sendCommand[20] = {0};
	char length[4] = {0};
		
	Temperature = CalculateTemperature();
	
	//float r = Temperature;
	//r += 0.005;
	
	//uint8_t IntPart = r;

	//Proper response, CORS header for cross-domain requests.
	strcpy(finalResult, "HTTP/1.1 200 OK"); //Add HTTP OK response header.
	strcat(finalResult, DEFAULT_APPENDER);
	strcat(finalResult, "Access-Control-Allow-Origin: *"); //Allow cross-over domain requests.
	//strcat(finalResult, DEFAULT_APPENDER);
	//strcat(finalResult, "Access-Control-Allow-Methods: GET,OPTIONS");
	strcat(finalResult, DEFAULT_APPENDER);
	strcat(finalResult, DEFAULT_APPENDER); //End of header section. 51chars
	
	//Load "inttypes.h" first.	
	//sprintf(tempValue, "%d.%d", IntPart, (uint8_t)((r - IntPart) * 10.0)); //OLD: Insert temperature as string by breaking the value to integer parts.
	//sprintf(tempValue, "%.1f", Temperature); //Alternative: Insert temperature as string directly using float characters (this required enabling the gcc options -Wl, -u, -vfprintf).
	//See: http://winavr.scienceprog.com/avr-gcc-tutorial/using-sprintf-function-for-float-numbers-in-avr-gcc.html
	
	dtostrf(Temperature, 3, 1, tempValue); //Use lightweight function to convert float to string. Minimum width (2nd parameter) including the "." character. Precision (3rd parameter).
	strcat(finalResult, tempValue); //Create final string.

	//Insert data length as string.
	sprintf(length, "%d", strlen(finalResult)); 

	//Build "prepare" string.
	strcpy(sendCommand, "AT+CIPSEND=");
	strcat(sendCommand, conn_string); //Link ID.
	strcat(sendCommand, ",");
	strcat(sendCommand, length); //Data length.
	//USARTWriteString("AT+CIPSEND=", 0, 0);
	//USARTWriteString(conn_string, 0, 0); //Link ID.
	//USARTWriteString(",", 0, 0);
	Send_StringUSART(sendCommand, 3, ">", 1, 0, 0); // \r\nXX.X
	//Send_StringUSART(DEFAULT_APPENDER, 1, ">", 0);

	//Send temperature value.
	Send_StringUSART(finalResult, 3, "SEND OK", 0, 0, 1);
}
void Close_Connection(char *conn_string)
{
	//Create "close" string.
	char close_string[20];
	strcpy(close_string, "AT+CIPCLOSE=");
	strcat(close_string, conn_string);

	//Close connection.
	Send_StringUSART(close_string, 3, "CLOSED", 1, 1, 0); //+IPD,0,345:GET /?TEMP=1 HTTP/1.1 Host: sdsddssd
}
//----------------------------------------------------------
float CalculateTemperature()
{
	uint16_t ADC_Read = 0; //Store final and filtered ADC reading here.
	double volts; //Voltage.
	float res; //Resistance.
	double val; //Temperature.

	ADC_Read = StartADCReading(); //Start reading ADC values.
		
	//Get voltage.
	volts = (ADC_Read * REF_VOLT) / 4095.0;

	//Calculate Resistance.
	res = (R0 * REF_VOLT) / volts - R0;
	
	//------------------------ Calculate temperature -----------------------------------
		
	//T = 1 / ( a + b * ln(res) + c * ln(res) ^ 3 )
	val = 1.0 / (a + b * log(res) + c * (log(res) * log(res) * log(res)));

	val -= 273.15; //Convert Kelvin to Celsius.
	
	//Save 1 decimal unit.
	val = val * 10.0;
	val = (int16_t)val / 10.0;
	
	//Display XXX.X while T >= 0
	//Display -XX.X while T < 0
	return (float)val;
}
uint16_t StartADCReading()
{
	/*
		For explanation of the following ADC Value Filtering,
		see Application Note AVR121: Enhancing ADC resolution by oversampling.

		TL;DR
		Accumulate 4^n 10-bit samples, where n is the desired extra number of bits in the resolution.
		Scale the accumulated result, by right shifting it n times.
	*/

	uint8_t i; //Counter of ADC values.
	uint16_t val = 0; //Store ADC reading here.
	uint16_t sum = 0; //Accumulator of ADC values.

	//Get ADC values.
	for(i = 0; i < BUFFERSIZE; i++)
	{
		val = ReadADC(ADC_PIN); //Read ADC.
		sum += val; //Sum ADC value.
	}

	val = (sum >> 2); //Scale accumulated result by shifting n=2 times.

	return val;
}
uint16_t ReadADC(uint8_t ch)
{
	//Select ADC Channel ch must be 0-7
	ch = ch&0b00000111;
	ADMUX = (ADMUX & 0xF8) | ch;
	
	_delay_us(10);

	//Start Single conversion
	ADCSRA |= (1 << ADSC);

	//Wait for conversion to complete
	while(!(ADCSRA & (1 << ADIF)));

	//Clear ADIF by writing one to it
	ADCSRA |= (1 << ADIF);

	return(ADC);
}
//----------------------------------------------------------
uint16_t ParseUInt16(char *string)
{
	uint8_t i = 0;
	uint16_t int_num = 0;
	char *head = string;
	char num[5] = {0};

	while(*head && i < 5)
	{
		if(isdigit(*head))
			num[i++] = *head;
		else if(i > 0)
			break;
		head++;
	}
	int_num = strtol(num, NULL, 10);
	
	return int_num;
}
//----------------------------------------------------------
void Wait(uint8_t wait_seconds)
{
	ResetTimer(); //Reset and start timer.

	delay = wait_seconds; //Set delay.
	
	while (!timeout){} //Wait until timeout.

	TCCR1B &= ~((1 << CS12)|(1 << CS10)); //Stop timer.
}
ISR(TIMER1_COMPA_vect)
{
	seconds++;
	
	if(seconds >= delay)
	{
		seconds = 0;
		timeout = 1;
		TCCR1B &= ~((1 << CS12)|(1 << CS10)); //Stop timer.
	}
}

//Use of timer makes the program miss a request sometimes.

//ISR(TIMER0_OVF_vect)
//{
	//milliseconds += 4;
	//
	//if(milliseconds >= 1000)
	//{
		//milliseconds = 0;
		//Temperature = CalculateTemperature();
	//}
//}
