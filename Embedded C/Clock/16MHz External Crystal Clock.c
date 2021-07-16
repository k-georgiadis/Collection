/*
MIT License
Copyright (c) 2021 Kosmas Georgiadis
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
/*
 * _16MHz_External_Crystal_Clock.c
 *
 * Created: 25/5/2015 8:39:30 μμ
 * Author: Kosmas Georgiadis
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>

volatile uint8_t digits[4]; //Digits to display.
volatile uint8_t i = 0;
volatile uint8_t db = 0; //Light dot?

volatile unsigned char clock_second = 0;
volatile unsigned char clock_minute = 0;
volatile unsigned char clock_hour = 0;

void init();
void InitTimer();
void SevenSegment(uint8_t n);
void Print(uint16_t num);

#define F_CPU 12000000UL;

void main()
{
	uint16_t time;
	init();
	InitTimer();
	
	while(1)
	{
		time = clock_minute * 100 + clock_second; //Calculate time.
		Print(time);
	}
}


void init()
{
	// Prescaler = FCPU/1024
	TCCR0 |= (1 << CS02);
	TIMSK |= (1 << TOIE0);
	TCNT0 = 0;
	
	//LED Display
	DDRB = 0x0F;
	DDRD = 0xFF;
	DDRC = 0X05;
	PORTC = 0X05;
	
	//Enable Global Interrupts
	sei();
}

void InitTimer()
{
	OCR1A = 15624;
	TCCR1B |= (1 << WGM12) | (1 << CS12) | (1 << CS10);
	TIMSK |= (1 << OCIE1A);
}

ISR(TIMER0_OVF_vect)
{
	if(i >= 3)
		i = 0;
	else
		i++;
	
	PORTD &= 0xF0; //Turn off all displays.
	PORTD |= (1 << i); //Light i-th display.

	SevenSegment(digits[i]); //Display digit.
}

ISR(TIMER1_COMPA_vect)
{
	clock_second++;
	
	if(clock_second >= 60)
	{
		clock_second = 0;
		clock_minute++;
		
		if(clock_minute >= 60)
		{
			clock_minute = 0;
			clock_hour++;
			
			if(clock_hour >= 10)
				clock_hour = 0;
		}
	}
}

void SevenSegment(uint8_t n)
{
	if(n < 10)
	{
		switch (n)
		{
			//PORTB = XXXXABCD
			//PORTD = EFG.XXXX
			case 0:
				PORTB |= 0b00001111;
				PORTD |= 0b11000000;
				PORTD &= 0b11001111;
				break;

			case 1:
				PORTB |= 0b00000110;
				PORTB &= 0b11110110;
				PORTD &= 0b00001111;
				break;

			case 2:
				PORTB |= 0b00001101;
				PORTB &= 0b11111101;
				PORTD |= 0b10100000;
				PORTD &= 0b10101111;
				break;

			case 3:
				PORTB |= 0b00001111;
				PORTD |= 0b00100000;
				PORTD &= 0b00101111;
				break;

			case 4:
				PORTB |= 0b00000110;
				PORTB &= 0b11110110;
				PORTD |= 0b01100000;
				PORTD &= 0b01101111;
				break;

			case 5:
				PORTB |= 0b00001011;
				PORTB &= 0b11111011;
				PORTD |= 0b01100000;
				PORTD &= 0b01101111;
				break;

			case 6:
				PORTB |= 0b00001011;
				PORTB &= 0b11111011;
				PORTD |= 0b11100000;
				PORTD &= 0b11101111;
				break;

			case 7:
				PORTB |= 0b00001110;
				PORTB &= 0b11111110;
				PORTD &= 0b00001111;
				break;

			case 8:
				PORTB |= 0b00001111;
				PORTD |= 0b11100000;
				PORTD &= 0b11101111;
				break;

			case 9:
				PORTB |= 0b00001111;
				PORTD |= 0b01100000;
				PORTD &= 0b01101111;
				break;
		}
	}
	else
	{
		PORTB |= 0b00001011;
		PORTB &= 0b11111011;
		PORTD |= 0b11110000;
	}
	
	//Light dot.
	if(i == 2 && db)
		PORTD |= (1 << 4);
}

void Print(uint16_t num)
{
	uint8_t i = 0;
	uint8_t j;
	
	if(num > 9999)
		return;
	
	while(num)
	{
		digits[i] = num % 10;
		i++;
		num = num / 10;
	}
	
	for(j = i; j < 4; j++)
		digits[j] = 0; //Clear rest of digits.
	
	db = 1; //Enable dot.
}

