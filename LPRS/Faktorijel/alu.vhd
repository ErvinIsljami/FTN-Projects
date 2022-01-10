library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

entity alu is
	port (
		iA		:	in std_logic_vector(15 downto 0);
		iB		:	in std_logic_vector(15 downto 0);
		iSEL	:  in std_logic_vector(3 downto 0);
		oC		: out std_logic_vector(15 downto 0);
		oZERO	: out std_logic;
		oSIGN	: out std_logic;
		oCARRY: out std_logic
	);
end entity;

architecture Behavioral of alu is

signal sREZ : std_logic_vector(16 downto 0);
signal sA : std_logic_vector(16 downto 0);
signal sB : std_logic_vector(16 downto 0);

begin	

sA <= '0'&iA;
sB <= '0'&iB;

process(iSEL, iA, iB, sA, sB) begin

	case(iSEL) is
		when "0000" =>	sREZ <= '0'&iA;
		when "0001" =>	sREZ <= sA + sB;
		when "0010" =>	sREZ <= ('0'&iA) - ('0'&iB);
		when "0011" =>	sREZ <= sA and sB;
		when "0100" =>	sREZ <= sA or sB;
		when "0101" =>	sREZ <= '0'&not(iA);
		when "0110" =>	sREZ <= sA + 1;
		when "0111" =>	sREZ <= sA - 1;
		when "1000" =>	sREZ <= sA(15 downto 0)&'0';
		when "1001" =>	sREZ <= sA(0)&sA(16 downto 1);
		when "1010" =>	sREZ <= '0'&(0 - iA);
		when "1011" =>	sREZ <= sA(0)&sA(15)&sA(15 downto 1);
		when others => sREZ <= (others => '0');
	end case;
	
end process;

oC <= sREZ(15 downto 0);
oCARRY <= sREZ(16);
oSIGN <= sREZ(15);
oZERO <= '1' when sREZ(15 downto 0) = "0000000000000000" else '0';


	
end architecture;