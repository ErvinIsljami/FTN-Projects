library ieee;
use ieee.std_logic_1164.all;

entity Reg is
	port (
		iCLK	:  in std_logic;
		iRST	:  in std_logic;
		iD		:  in std_logic_vector(15 downto 0);
		iWE	:  in std_logic;
		oQ		: out std_logic_vector(15 downto 0)
	);
end entity;

architecture Behavioral of Reg is


signal sREG : std_logic_vector(15 downto 0);

begin

process(iCLK, iRST) begin
	if(iRST = '1') then
		sREG <= (others => '0');
	elsif(iCLK'event and iCLK = '1') then
		if(iWE = '1') then
			sREG <= iD;
		else
			sREG <= sREG;
		end if;
	end if;
end process;

oQ <= sREG;
	
end architecture;		