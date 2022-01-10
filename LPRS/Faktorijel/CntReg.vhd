library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

entity CntReg is
	port (
		iCLK	:  in std_logic;
		iRST	:  in std_logic;
		iEN	:  in std_logic;
		oQ		: out std_logic_vector(15 downto 0)
	);
end entity;

architecture Behavioral of CntReg is

signal sCNT : std_logic_vector(15 downto 0);

begin

process(iCLK, iRST) begin
	if(iRST = '1') then
		sCNT <= (others => '0');
	elsif(iCLK'event and iCLK = '1') then
		if(iEN = '1') then
			sCNT <= sCNT + 1;
		else
			sCNT <= sCNT;
		end if;
	end if;
end process;

oQ <= sCNT;

end architecture;		