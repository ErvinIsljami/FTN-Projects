library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

entity cu is
	port (
		iCLK		:  in std_logic;
		iRST		:  in std_logic;
		iZERO		:	in std_logic;
		iSIGN		:  in std_logic;
		iCARRY	:	in std_logic;
		oREG_WE	: out std_logic_vector(7 downto 0);
		oMUXA_SEL: out std_logic_vector(3 downto 0);
		oMUXB_SEL: out std_logic_vector(3 downto 0);
		oALU_SEL	: out std_logic_vector(3 downto 0)
	);
end entity;

architecture Behavioral of cu is

type tState is (START, DONE, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10);
signal currentState, nextState : tState;

begin

process(iCLK, iRST) begin
	if(iRST = '1') then
		currentState <= START;
	elsif(iCLK'event and iCLK = '1') then
		currentState <= nextState;
	end if;
end process;

process(currentState, iSIGN, iZERO) begin

	case(currentState) is
		when START 	=>	nextState <= T1;
		when T1 		=>	nextState <= T2;
		when T2		=> nextState <= T3;
		when T3		=> if(iZERO = '1') then
								nextState <= DONE;
							else
								nextState <= T4;
							end if;
		when T4		=> nextState <= T5;
		when T5		=> if(iSIGN = '1') then
								nextState <= T6;
							elsif(iSIGN = '0') then
								nextState <= T8;
							else
								nextState <= T8;
							end if;
		when T6		=> nextState <= T7;
		when T7		=> nextState <= T5;
		when T8		=> nextState <= T9;
		when T9		=> nextState <= T10;
		when T10		=> nextState <= T3;
		when others => nextState <= DONE;
	end case;
	
end process;


process(currentState) begin

	case(currentState) is
		when START 	=>	oREG_WE <= "00000000"; oALU_SEL <= "0000"; oMUXA_SEL <= "0000"; oMUXB_SEL <= "0000"; --START
		when T1 		=>	oREG_WE <= "00000100"; oALU_SEL <= "0000"; oMUXA_SEL <= "1000"; oMUXB_SEL <= "0000"; --R2<-n
		when T2		=> oREG_WE <= "00001000"; oALU_SEL <= "0111"; oMUXA_SEL <= "0010"; oMUXB_SEL <= "0000"; --R3<-R2-1
		when T3		=> oREG_WE <= "00001000"; oALU_SEL <= "0000"; oMUXA_SEL <= "0011"; oMUXB_SEL <= "0000"; --R3<-R3
		when T4		=> oREG_WE <= "00000001"; oALU_SEL <= "0010"; oMUXA_SEL <= "0000"; oMUXB_SEL <= "0000"; --R0<-R0-R0
		when T5		=> oREG_WE <= "00000000"; oALU_SEL <= "0010"; oMUXA_SEL <= "0001"; oMUXB_SEL <= "0011"; --R1 - R3
		when T6		=> oREG_WE <= "00000001"; oALU_SEL <= "0001"; oMUXA_SEL <= "0000"; oMUXB_SEL <= "0010"; --R0<-R0 + R2
		when T7		=> oREG_WE <= "00000010"; oALU_SEL <= "0110"; oMUXA_SEL <= "0001"; oMUXB_SEL <= "0000"; --R1<-R1 + 1
		when T8		=> oREG_WE <= "00000010"; oALU_SEL <= "0010"; oMUXA_SEL <= "0001"; oMUXB_SEL <= "0001"; --R1<-R1 - R1
		when T9		=> oREG_WE <= "00001000"; oALU_SEL <= "0111"; oMUXA_SEL <= "0011"; oMUXB_SEL <= "0000"; --R3<-R3 - 1
		when T10		=> oREG_WE <= "00000100"; oALU_SEL <= "0000"; oMUXA_SEL <= "0000"; oMUXB_SEL <= "0000"; --R2<-R0
		when others => oREG_WE <= "00000000"; oALU_SEL <= "0000"; oMUXA_SEL <= "0000"; oMUXB_SEL <= "0000"; --DONE
	end case;
end process;




	
end architecture;