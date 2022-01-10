library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

entity cnt is
	port (
		iCLK	:  in std_logic;
		iRST	:  in std_logic;
		iD		:	in std_logic_vector(15 downto 0);
		iEN	:  in std_logic;
		iLOAD	:	in std_logic;
		oQ		: out std_logic_vector(15 downto 0)
	);
end entity;

architecture Behavioral of cnt is

	signal sCNT : std_logic_vector(15 downto 0);

begin

	process(iCLK, iRST) begin
		if(iRST = '1') then
			sCNT <= (others => '0');
		elsif(rising_edge(iCLK)) then
			if(iEN = '1') then
				if(iLOAD = '1') then
					sCNT <= iD;
				else
					sCNT <= sCNT + 1;
				end if;
			else
				sCNT <= sCNT;
			end if;
		end if;
	end process;
	
	oQ <= sCNT;

end architecture;