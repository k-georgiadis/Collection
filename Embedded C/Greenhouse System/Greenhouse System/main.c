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
 * Created: 8/7/2016 8:53:55 PM
 * Author : CosmaOne

	A small greenhouse system with timed relays and internal clock.
 */

 #define F_CPU 8000000UL

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <avr/eeprom.h>

#define MAX_PAGES	4  //{R1/2 status, Clock, R1 settings, R2 settings}
#define MAX_RELAYS	2

const char ON_STRING[]	= "ON";
const char OFF_STRING[] =	"OFF";

typedef enum STATUS
{
	ON = 1,
	OFF = 0
} STATUS;

typedef enum MODE
{
	M_TIME = 0,
	M_RELAY = 1
} MODE;

static void Port_Init();
static void LCD_Init();
static void TimerInit();
static void GetEEPROM_Values();

static void LoadCustomCharacters();
static void LoadAnimation(char *string);
static void EraseAnimation();

static char Keypad(char *string, uint8_t IsPIN, uint8_t MaxLength);
static uint8_t CheckPIN(char *string);

static void Pages(uint8_t pageNum);
static uint8_t ChangePage(char input);
static void MoveCursor(uint8_t direction, MODE _mode);

static void ChangeTime();
static void SetTime();

static void SetRelayTime();
static void SetRelayStatus();

static void SetTimeVariables(char input, MODE _mode);

static void lcd_cmd(unsigned char command);
static void lcd_data(unsigned char data);
static void lcd_send_string(char *string, uint8_t IsDelayed);

static void Timer1(STATUS status);
static void Timer2(STATUS status, uint16_t timeout);

/*	LCD Info.

	Row1      0x80 ... 0x8F     	RS=PB1
	Row2      0xCO ... 0xCF			R/W=PB2 //GROUND IT!!!!
									E=PB0
	
*/

//4x3 Keyboard Matrix.
char row_col [4][3] = {
						  {'1','2','3'},
						  {'4','5','6'},
						  {'7','8','9'},
						  {'*','0','#'}
					  };

//EEPROM relay statuses.
STATUS EEMEM eep_relay_status[MAX_RELAYS];
		
//Default relay statuses and strings.			   
volatile STATUS relay_status[MAX_RELAYS]	= {OFF, OFF};
char relay_strings[MAX_RELAYS][20]			= {"Relay 1:     ", "Relay 2:     "};
	
//Time values for relays in EEPROM.
uint8_t EEMEM eep_relay_times[MAX_RELAYS][2][2];

//Default time values for relays.
uint8_t relay_times[MAX_RELAYS][2][2]		= {
												 {{15, 1}, {15, 2}}, //R1 ON/OFF.
												 {{15, 2}, {15, 3}}	 //R2 ON/OFF.
											  };
//Default page.
uint8_t current_page		= 1;
volatile uint8_t hidePress	= 0; //This prevents the "Press 0" text from printing again.

//EEPROM clock values.
EEMEM uint8_t eep_second;
EEMEM uint8_t eep_minute;
EEMEM uint8_t eep_hour;

//Default clock values.
volatile uint8_t second			= 0;
volatile uint8_t minute			= 0;
volatile uint8_t hour			= 15;
char current_time[16];

uint8_t SetingRelayTime			= 0;
uint8_t RelayTimeSet			= 0;
uint8_t SetingTime				= 0;

volatile uint8_t timed_out		= 0;
volatile uint16_t holdTime		= 0;
volatile uint16_t milliseconds	= 0;

volatile uint8_t CursorAddress;
uint8_t access					= 0;

char input_pressed				= 0;
uint8_t key_pressed[2]			= {0};

int main(void)
{
    Port_Init();
	LCD_Init();
		
	GetEEPROM_Values();

	TimerInit(); //Start clock.

	LoadCustomCharacters();
	LoadAnimation("!Loading");
	EraseAnimation();
	
	char *string = calloc(5, sizeof(char)); //PIN String = 5 characters, including \0.	
	lcd_send_string("Enter PIN: ", 0);

	while(!access)
	{
		Keypad(string, 1, 4);
		access = CheckPIN(string);
	}
		
	Pages(1); //Display main page.
	
	while(1)
    {
		Keypad(string, 0, 0);
	}
} 

void Port_Init()
{
	//LCD init.
	DDRD = 0xFF; //Set PORTD as Output.
	DDRB = 0x3F; //Set PB0,PB1 as Output and Keypad columns as output.
	
	//Keypad init.
	DDRC = 0xF0; //Rows as input, Relays as output.
	PORTC = 0x0F; //Enable pull-ups on rows.
	
	ADCSRA &= ~(1 << ADEN); //Disable ADC.
	ACSR = (1 << ACD); //Disable the analog comparator.
}
void LCD_Init()
{
	//Setup both lines of LCD.
	lcd_cmd(0x38);
	
	//Set Cursor off - Enable LCD.
	lcd_cmd(0x0C);
	
	//Clear Screen.
	lcd_cmd(0x01);
	
	//Line 1, 1st position.
	lcd_cmd(0x80);
}
void TimerInit()
{
	//Timer1
	TCCR1B = (1 << WGM12); // Mode 4, CTC on OCR1A.
	TIMSK |= (1 << OCIE1A); //Set interrupt on compare match.
	OCR1A = 31249; //1 second. F_CPU = 8MHz, P = 256
	TCCR1B |= (1 << CS12); //Set prescaler 256 and start Timer1.
	
	//Timer2
	TCCR2 |= (1 << WGM21); //Set to CTC Mode.
	TIMSK |= (1 << OCIE2);  //Set interrupt on compare match.
	OCR2 = 125; //1 ms.
	
	sei();
}
void GetEEPROM_Values()
{
	uint8_t i, j;
	uint8_t h, m, s;
	
	//Read EEPROM clock values, if available.
	eeprom_busy_wait();
	h = eeprom_read_byte(&eep_hour);
	eeprom_busy_wait();
	m = eeprom_read_byte(&eep_minute);
	eeprom_busy_wait();
	s = eeprom_read_byte(&eep_second);

	if(h != 0xFF)
		hour = h;
	if(m != 0xFF)
		minute = m;
	if(s != 0xFF)
		second = s;

	//Read EEPROM relay time values, if available.
	for(i = 0; i < MAX_RELAYS; i++)
	{
		for(j = 0; j < 2; j++)
		{
			eeprom_busy_wait();
			h = eeprom_read_byte(eep_relay_times[i][j]);
			eeprom_busy_wait();
			m = eeprom_read_byte(eep_relay_times[i][j] + 1);

			if(h != 0xFF)
				relay_times[i][j][0] = h;
			if(m != 0xFF)
				relay_times[i][j][1] = m;
		}
	}

	//Read EEPROM relay statuses, if available.
	for(i = 0; i < MAX_RELAYS; i++)
	{
		eeprom_busy_wait();
		h = eeprom_read_byte(eep_relay_status + i);

		if(h != 0xFF)
		{
			relay_status[i] = h;
			PORTC |= (0x10 << h);
		}
	}

	//Insert terminating char to strings.
	relay_strings[0][13] = 0;
	relay_strings[1][13] = 0;

	return;
}

void LoadCustomCharacters()
{
	lcd_cmd(0x40); //Go to CG RAM, in address 0x00.
	
	//Load first custom character. 0x00
	lcd_data(0x1F);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_cmd(0x48);
	
	//Load second custom character. 0x01
	lcd_data(0x1F);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	
	//Load third custom character. 0x02
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	
	//Load fourth custom character. 0x03
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x01);
	lcd_data(0x1F);
	
	//Load fifth custom character. 0x04
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x00);
	lcd_data(0x1F);

	//Load sixth custom character. 0x05
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x1F);
	
	//Load seventh custom character. 0x06
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	
	//Load eighth custom character. 0x07
	lcd_data(0x1F);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	lcd_data(0x10);
	
	lcd_cmd(0x80); //Return to DD RAM and first line.
}
void LoadAnimation(char *string)
{
	uint8_t i,j;
	uint8_t length = strlen(string);
	
	double delay = 56.0;
	
	//Rows.
	for(j = 1; j <= 2; j++)
	{
		//Columns.
		for(i = 1; i <= 16; i++)
		{
			//First row.
			if(j == 1)
			{
				lcd_data(0x00); //Display top line.
				_delay_ms(delay);
				
				if(i == (16 - length) / 2)
				{
					lcd_send_string(string, 1);
					i = i + length;
				}
			}
			else
			{
				//Second row.
				_delay_ms(delay - 4.0); //Delay.
				lcd_cmd(0xCF - i); //Go backwards.
				lcd_data(0x04); //Display bottom line.
			}
		}
		
		//Changing row.
		if(j == 1)
		{
			//_delay_ms(delay - 4); //Delay. (Minus the cmd delay below)
			lcd_cmd(0x8F); //Go to first line, last position.
			lcd_data(0x01); //Display corner.
			_delay_ms(delay - 4.0); //Delay. (Minus the cmd delay below)
			
			lcd_cmd(0xCF); //Go to second line, last position.
			lcd_data(0x02); //Display right line.
			_delay_ms(delay - 4.0); //Delay.
			
			lcd_cmd(0xCF); //Go to second line, last position again.
			lcd_data(0x03); //Display corner.
		}
		else
		{
			lcd_cmd(0xC0); //Go second line, first position.
			lcd_data(0x05); //Display corner.
			
			_delay_ms(delay - 4.0); //Delay. (Minus the cmd delay below)
			lcd_cmd(0x80); //Go to first line, first position.
			lcd_data(0x07); //Display corner.
		}
	}
}
void EraseAnimation()
{
	uint8_t i,j;
	double delay = 56.0;
	
	lcd_cmd(0x80);
	_delay_ms(500);
	
	//Rows.
	for(j = 1; j <= 2; j++)
	{
		//Columns.
		for(i = 1; i <= 16; i++)
		{
			//First row.
			if(j == 1)
			{
				if(i == 16)
				{
					lcd_data(0x02);
					_delay_ms(delay - 4.0);
				}
				else if(i == 1)
				{
					lcd_data(0x06);
					_delay_ms(delay);
				}
				else
				{
					lcd_data(' ');
					_delay_ms(delay);
				}
			}
			else
			{
				//Second row.
				lcd_cmd(0xCF - i); //Go backwards.
				_delay_ms(delay - 4.0);
				
				if(i==16)
				lcd_data(0x06);
				else
				lcd_data(' ');
			}

		}

		//Changing row.
		if(j == 1)
		{
			lcd_cmd(0x8F); //Go to first line, last position.
			lcd_data(' ');
			
			_delay_ms(delay - 4.0);
			lcd_cmd(0xCF); //Go to second line, last position.
			lcd_data(0x04);
			
			_delay_ms(delay - 4.0);
			lcd_cmd(0xCF); //Go to second line, last position again.
			lcd_data(' ');
		}
		else
		{
			lcd_cmd(0xC0); //Go second line, first position.
			lcd_data(' ');
			
			_delay_ms(delay - 4.0);
			lcd_cmd(0x80); //Go to first line, first position.
			lcd_data(' ');
			lcd_cmd(0x80);
		}

	}
}

char Keypad(char *string, uint8_t IsPIN, uint8_t MaxLength)
{
	char input;
	int8_t i, j;
	uint8_t index = strlen(string);
	
	for(j = 2; j >= 0; j--) //Col0 = PB3.
	{
		PORTB = ~(8 << j); //Check each column by feeding with 0.
		_delay_ms(1); //A delay is necessary, otherwise it won't work.

		for(i = 3; i >= 0; i--) //Row0 = PC3.
		{
			input = row_col[i][j];

			if(!(PINC & (1 << i)))
			{
				//Save key data.
				key_pressed[0] = i;
				key_pressed[1] = j;

				input_pressed = input;

				//If '0' button was pressed at a time page, start Timer2 once.
				if(!IsPIN && input_pressed == '0' && current_page >= 2 && SetingTime && !(TCCR2 & (1 << CS22)))
					Timer2(1, 500);
			}
			else if(input_pressed && i == key_pressed[0] && j == key_pressed[1])
			{
				if(IsPIN && input_pressed != '*' && input_pressed != '#' && index < MaxLength)
				{				
					string[index++] = input_pressed;
					string[index] = 0;

					lcd_data('*'); //Show char.
				}
				else if(input_pressed == '0' && !SetingTime)
				{
					 input_pressed = 0; //Don't count input.
					 
					 if(current_page == 2)
						ChangeTime();
					 else if(current_page > 2)
					 {
						lcd_cmd(0x0E); //Show cursor at relay times.
						SetingTime = 1;
					 }
				}
				else if(input_pressed == '0' && timed_out) //If '0' button was held for a specific time, save changes.
				{					
					if(current_page == 2)
						SetTime();
					else if(current_page > 2)
						SetRelayTime();
				}
				else
					Timer2(0, 0);				
							
				if(!ChangePage(input_pressed))
				{
					if(SetingTime)
					{
						if(current_page == 2) //Edit clock.
							SetTimeVariables(input_pressed, M_TIME);
						else if(current_page > 2) //Edit relay times.
							SetTimeVariables(input_pressed, M_RELAY);
					}
				}
				input_pressed = 0;
			}
		}	
	}
						
	return 0;
}
uint8_t CheckPIN(char *string)
{
	if(strlen(string) >= 4)
	{
		if(strcmp(string, "1234"))
		{
			lcd_cmd(0xC0);
			lcd_send_string("Incorrect PIN.", 0);
			
			string[0] = 0; //Reset string.
			lcd_cmd(0x8B); //Move back.
			lcd_send_string("    ", 0); //Erase old PIN.
			lcd_cmd(0x8B); //Move back again.
		}
		else
		{
			lcd_cmd(0xC0);
			lcd_send_string("Access Granted!", 0);
			_delay_ms(1000); //Delay before entering.

			return 1; //Success.
		}
	}

	return 0;
}

void Pages(uint8_t pageNum)
{	
	if(pageNum == 1) //Main page. Relay status.
	{
		lcd_cmd(0x01); //Clear screen.
		lcd_cmd(0x0C); //Hide cursor.		
		hidePress = 0;

		lcd_cmd(0x80);
		lcd_send_string(relay_strings[0], 0);
		lcd_send_string(relay_status[0] ? ON_STRING : OFF_STRING, 0);

		lcd_cmd(0xC0);
		lcd_send_string(relay_strings[1], 0);
		lcd_send_string(relay_status[1] ? ON_STRING : OFF_STRING, 0);
	}
	else if(pageNum == 2) //Clock.
	{	
		lcd_cmd(0x0C); //Hide cursor.

		//Print text only one time.
		if(!hidePress)
		{
			lcd_cmd(0x01); //Clear the screen.		
			lcd_cmd(0xC0);
			lcd_send_string("Reset (Press 0)", 0);
			hidePress = 1;
		}

		lcd_cmd(0x80);
		
		//Format time (hh:mm:ss).
		sprintf(current_time, "    %02d:%02d:%02d    ", hour, minute, second);
		lcd_send_string(current_time, 0); //Print it.	
	}
	else if(pageNum == 3) //R1 settings.
	{
		hidePress = 0;
		
		lcd_cmd(0x80);
		sprintf(current_time, "R1  ON:    %02d:%02d", relay_times[0][0][0], relay_times[0][0][1]);
		lcd_send_string(current_time, 0);
			
		lcd_cmd(0xC0);
		sprintf(current_time, "R1 OFF:    %02d:%02d", relay_times[0][1][0], relay_times[0][1][1]);
		lcd_send_string(current_time, 0);
		
		lcd_cmd(0x8B); //Move to time.
		
		CursorAddress = 0x8B;
	}
	else if(pageNum == 4) //R2 settings.
	{
		lcd_cmd(0x80);
		sprintf(current_time, "R2  ON:    %02d:%02d", relay_times[1][0][0], relay_times[1][0][1]);
		lcd_send_string(current_time, 0);

		lcd_cmd(0xC0);
		sprintf(current_time, "R2 OFF:    %02d:%02d", relay_times[1][1][0], relay_times[1][1][1]);
		lcd_send_string(current_time, 0);
		
		lcd_cmd(0x8B); //Move to time.
		
		CursorAddress = 0x8B;
	}

	return;
}
uint8_t ChangePage(char input)
{
	MODE _mode = 0;
	
	if(current_page == 2)
		_mode = M_TIME;
	else if(current_page > 2)
		_mode = M_RELAY;

	if(input == '*')
	{
		if(!SetingTime)
		{
			if(current_page <= 1)
				current_page = MAX_PAGES;
			else
				current_page--;
			
			Pages(current_page); //Change page.
		}
		else
			MoveCursor(0, _mode); //Move cursor to the left.
		
		return 1;
	}
	else if(input == '#')
	{
		if(!SetingTime)
		{
			if(current_page >= MAX_PAGES)
				current_page = 1;
			else
				current_page++;
			
			Pages(current_page); //Change page.
		}
		else
			MoveCursor(1, _mode); //Move cursor to the right.
		
		return 1;
	}
	
	return 0;
}
void MoveCursor(uint8_t direction, MODE _mode)
{
	if(direction == 1)
	{
		CursorAddress++; //Increment address.

		if(_mode == M_TIME)
		{
			if(CursorAddress == 0x86 || CursorAddress == 0x89)
				CursorAddress++;
			else if(CursorAddress > 0x8B) //If we are out of bounds.
				CursorAddress = 0x84; //Reset address.
		}
		else if(_mode == M_RELAY)				
		{
			if(CursorAddress == 0x8D)
				CursorAddress++;
			else if(CursorAddress == 0x90)
				CursorAddress = 0xCB;
			else if(CursorAddress == 0xCD)	
				CursorAddress++;
			else if(CursorAddress == 0xD0)
				CursorAddress = 0x8B;
		}
	}
	else
	{
		CursorAddress--; //Decrement address.
				
		if(_mode == M_TIME)
		{
			if(CursorAddress == 0x86 || CursorAddress == 0x89)
				CursorAddress--;
			else if(CursorAddress < 0x84) //If we are out of bounds.
				CursorAddress = 0x8B; //Set last address.
		}		
		else if(_mode == M_RELAY)
		{
			if(CursorAddress == 0x8A)
				CursorAddress = 0xCF;
			else if(CursorAddress == 0x8D)
				CursorAddress--;
			else if(CursorAddress == 0xCA)
				CursorAddress = 0x8F;
			else if(CursorAddress == 0xCD)
				CursorAddress--;
		}	
	}
	
	lcd_cmd(CursorAddress);

	return;
}

void ChangeTime()
{
	SetingTime = 1;

	//Disable timer1.
	Timer1(0);
	
	lcd_cmd(0xC0);
	lcd_send_string("Return (Hold 0) ", 0); //Change text.
	
	//Reset time variables.
	second = 0;
	minute = 0;
	hour = 0;
	
	lcd_cmd(0x84);
	lcd_send_string("00:00:00", 0);
	lcd_cmd(0x0E); //Show cursor.

	CursorAddress = 0x84;
	lcd_cmd(CursorAddress);

	return;
}
void SetTime()
{
	Timer2(0, 0);
		
	SetingTime = 0; //Reset time flag.
	lcd_cmd(0x0C); //Hide cursor.
			
	lcd_cmd(0xC0);
	lcd_send_string("Reset (Press 0)", 0);
	
	Pages(2); //Refresh page.
	Timer1(1); //Start timer.
	
	return;
}

void SetRelayTime()
{
	Timer2(0, 0);
	
	SetingTime = 0; //Reset time flag.
	lcd_cmd(0x0C); //Hide cursor.
	
	Pages(current_page); //Refresh page.
	
	return;
}
void SetRelayStatus()
{
	char flag = 0;
	uint8_t i;

	for(i = 0; i < MAX_RELAYS; i++)
	{
		if(relay_times[i][0][0] == hour && relay_times[i][0][1] == minute && relay_status[i] == OFF)
		{
			relay_status[i] = ON;
			PORTC |= (0x10 << i); //Start Relay.
			flag = 1;

			eeprom_busy_wait();
			eeprom_update_byte(eep_relay_status + i, ON);
		}
		else if(relay_times[i][1][0] == hour && relay_times[i][1][1] == minute && relay_status[i] == ON)
		{
			relay_status[i] = OFF;
			PORTC &= ~(0x10 << i); //Stop Relay.
			flag = 1;

			eeprom_busy_wait();
			eeprom_update_byte(eep_relay_status + i, OFF);
		}
	}

	if(access && flag && current_page == 1)
		Pages(1); //Refresh main page.

	return;
}

void SetTimeVariables(char input, MODE _mode)
{
	if(!input)
		return;

	lcd_data(input); //Print number.
	CursorAddress++;

	if(_mode == M_TIME)
	{
		input -= '0';

		if(CursorAddress == 0x85)
			hour = 10 * input + hour % 10;
		else if(CursorAddress == 0x86)
		{
			hour = 10 * (hour / 10) + input;

			CursorAddress = 0x87;
			lcd_cmd(CursorAddress);
		}
		else if(CursorAddress == 0x88)
			minute = 10 * input + minute % 10;
		else if(CursorAddress == 0x89)
		{
			minute = 10 * (minute / 10) + input;

			CursorAddress = 0x8A;
			lcd_cmd(CursorAddress);
		}
		else if(CursorAddress == 0x8B)
			second = 10 * input + second % 10;
		else if(CursorAddress == 0x8C)
		{
			second = 10 * (second / 10) + input;

			CursorAddress = 0x84;
			lcd_cmd(CursorAddress);
		}

		//Check if values are valid.
		if(hour > 23)
		{
			hour = 23;
			lcd_cmd(0x84);
			lcd_data('2');
			lcd_data('3');
			lcd_cmd(CursorAddress);
		}
		else if(minute > 59)
		{
			minute = 59;
			lcd_cmd(0x87);
			lcd_data('5');
			lcd_data('9');
			lcd_cmd(CursorAddress);
		}
		else if(second > 59)
		{
			second = 59;
			lcd_cmd(0x8A);
			lcd_data('5');
			lcd_data('9');
			lcd_cmd(CursorAddress);
		}

		//Save values to EEPROM.
		eeprom_busy_wait();
		eeprom_update_byte(&eep_hour, hour);
		eeprom_busy_wait();
		eeprom_update_byte(&eep_minute, minute);
		eeprom_busy_wait();
		eeprom_update_byte(&eep_second, second);

	}
	else if(_mode == M_RELAY)
	{
		uint8_t relay_index = current_page - 3;

		if(CursorAddress == 0x8C)
			relay_times[relay_index][0][0] = 10 * (input - '0') + relay_times[relay_index][0][0] % 10;
		else if(CursorAddress == 0x8D)
		{
			relay_times[relay_index][0][0] = 10 * (relay_times[relay_index][0][0] / 10) + input - '0';

			CursorAddress = 0x8E;
			lcd_cmd(CursorAddress);
		}
		else if(CursorAddress == 0x8F)
			relay_times[relay_index][0][1] = 10 * (input - '0') + relay_times[relay_index][0][1] % 10;
		else if(CursorAddress == 0x90)
		{
			relay_times[relay_index][0][1] = 10 * (relay_times[relay_index][0][1] / 10) + input - '0';

			CursorAddress = 0xCB;
			lcd_cmd(CursorAddress);
		}
		else if(CursorAddress == 0xCC)
			relay_times[relay_index][1][0] = 10 * (input - '0') + relay_times[relay_index][1][0] % 10;
		else if(CursorAddress == 0xCD)
		{
			relay_times[relay_index][1][0] = 10 * (relay_times[relay_index][1][0] / 10) + input - '0';

			CursorAddress = 0xCE;
			lcd_cmd(CursorAddress);
		}
		else if(CursorAddress == 0xCF)
			relay_times[relay_index][1][1] = 10 * (input - '0') + relay_times[relay_index][1][1] % 10;
		else if(CursorAddress == 0xD0)
		{
			relay_times[relay_index][1][1] = 10 * (relay_times[relay_index][1][1] / 10) + input - '0';

			CursorAddress = 0x8B;
			lcd_cmd(CursorAddress);
		}
	
		//Check if values are valid.
		if(relay_times[relay_index][0][0] > 23)
		{
			relay_times[relay_index][0][0] = 23;
			lcd_cmd(0x8B);
			lcd_data('2');
			lcd_data('3');
			lcd_cmd(CursorAddress);
		}
		else if(relay_times[relay_index][0][1] > 59)
		{
			relay_times[relay_index][0][1] = 59;
			lcd_cmd(0x8E);
			lcd_data('5');
			lcd_data('9');
			lcd_cmd(CursorAddress);
		}
		if(relay_times[relay_index][1][0] > 23)
		{
			relay_times[relay_index][1][0] = 23;
			lcd_cmd(0xCB);
			lcd_data('2');
			lcd_data('3');
			lcd_cmd(CursorAddress);
		}
		else if(relay_times[relay_index][1][1] > 59)
		{
			relay_times[relay_index][1][1] = 59;
			lcd_cmd(0xCE);
			lcd_data('5');
			lcd_data('9');
			lcd_cmd(CursorAddress);
		}

		//Check if ON and OFF times are equal.
		if(relay_times[relay_index][0][0] == relay_times[relay_index][1][0] && relay_times[relay_index][0][1] == relay_times[relay_index][1][1])
		{
			relay_times[relay_index][1][1]++; //Increment by one second to differentiate.
			Pages(current_page); //Refresh page.
		}

		//Save values to EEPROM.
		eeprom_busy_wait();
		eeprom_update_block(relay_times[relay_index][0], eep_relay_times[relay_index][0], sizeof(relay_times[relay_index][0])); //ON time.
		eeprom_busy_wait();
		eeprom_update_block(relay_times[relay_index][1], eep_relay_times[relay_index][1], sizeof(relay_times[relay_index][1])); //OFF time.
	}

	return;
}

void lcd_cmd(unsigned char command)
{
	//Put command on the Data Bus.
	PORTD = command;

	//Enable LCD for command writing.
	PORTB = (PORTB & 0xFC) | 0x01; //Get columns status before changing.
	_delay_ms(2);

	//Disable LCD again.
	PORTB &= 0xFC;
	_delay_ms(2);
}
void lcd_data(unsigned char data)
{
	//Put data on Data Bus.
	PORTD = data;
	
	//Set R/S (Register Select) to High, and Enable to High.
	PORTB |= 0x03; //Get columns status before changing.
	
	_delay_ms(2);

	//Disable LCD again.
	PORTB &= 0xFC;
	_delay_ms(2);
}
void lcd_send_string(char* string, uint8_t IsDelayed)
{
	while(*string)
	{
		lcd_data(*string);
		
		if(IsDelayed)
			_delay_ms(56.0);
		string++;
	}
}

void Timer1(STATUS status)
{
	if(status == ON)
	{
		TCNT1 = 0;
		TCCR1B |= (1 << CS12); //Start timer. Prescaler = 64.
	}
	else if(status == OFF)
		TCCR1B &= ~(1 << CS12); //Stop timer.

	return;
}
void Timer2(STATUS status, uint16_t timeout)
{
	if(status == ON)
	{
		holdTime = timeout;
		timed_out = 0;
		TCNT2 = 0;
		TCCR2 |= (1 << CS22); //Start timer. Prescaler = 64.
	}
	else if(status == OFF)
	{
		milliseconds = 0;
		holdTime = 0;
		TCCR2 &= ~(1 << CS22); //Stop timer.
	}
	
	return;
}
ISR(TIMER1_COMPA_vect)
{
	second++;
	if(second >= 60)
	{
		second = 0;
		minute++;
		
		if(minute >= 60)
		{
			minute = 0;
			hour++;
			
			if(hour >= 24)
				hour = 0;
		}
	}

	SetRelayStatus();

	if(current_page == 2)
		Pages(2);
	
}
ISR(TIMER2_COMP_vect)
{
	milliseconds++;

	//Check limit.
	if(milliseconds >= holdTime)
	{
		milliseconds = holdTime;
		timed_out = 1;
	}
}

