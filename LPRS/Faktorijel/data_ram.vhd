library ieee;
use ieee.std_logic_1164.all;

entity data_ram is
	port (
		iCLK	:  in std_logic;
		iRST	:  in std_logic;
		iA		:	in std_logic_vector(4 downto 0);
		iD		:  in std_logic_vector(15 downto 0);
		iWE	:	in std_logic;
		oQ		: out std_logic_vector(15 downto 0)
	);
end entity;

architecture Behavioral of data_ram is

	type ram_array is array (0 to 31) of std_logic_vector (15 downto 0);
	
	constant cram : ram_array := (
		0 => "0000000000000000", 
		1 => "0000000000000000", 
		2 => "0000000000000000", 
		3 => "0000000000000000", 
		4 => "0000000000000000", 
		5 => "0000000000000000", 
		6 => "0000000000000000", 
		7 => "0000000000000000", 
		8 => "0000000000000000", 
		9 => "0000000000000000", 
		10 => "0000000000000000", 
		11 => "0000000000000000", 
		12 => "0000000000000000", 
		13 => "0000000000000000", 
		14 => "0000000000000000", 
		15 => "0000000000000000", 
		16 => "0000000000000000", 
		17 => "0000000000000000", 
		18 => "0000000000000000", 
		19 => "0000000000000000",
		20 => "0000000000000000", 
		21 => "0000000000000000", 
		22 => "0000000000000000", 
		23 => "0000000000000000", 
		24 => "0000000000000000", 
		25 => "0000000000000000", 
		26 => "0000000000000000", 
		27 => "0000000000000000", 
		28 => "0000000000000000", 
		29 => "0000000000000000", 
		30 => "0000000000000000", 
		31 => "0000000000000000" 
	);
	
	signal ram : ram_array := (
		0 => "0000000000000000", 
		1 => "0000000000000000", 
		2 => "0000000000000000", 
		3 => "0000000000000000", 
		4 => "0000000000000000", 
		5 => "0000000000000000", 
		6 => "0000000000000000", 
		7 => "0000000000000000", 
		8 => "0000000000000000", 
		9 => "0000000000000000", 
		10 => "0000000000000000", 
		11 => "0000000000000000", 
		12 => "0000000000000000", 
		13 => "0000000000000000", 
		14 => "0000000000000000", 
		15 => "0000000000000000", 
		16 => "0000000000000000", 
		17 => "0000000000000000", 
		18 => "0000000000000000", 
		19 => "0000000000000000",
		20 => "0000000000000000", 
		21 => "0000000000000000", 
		22 => "0000000000000000", 
		23 => "0000000000000000", 
		24 => "0000000000000000", 
		25 => "0000000000000000", 
		26 => "0000000000000000", 
		27 => "0000000000000000", 
		28 => "0000000000000000", 
		29 => "0000000000000000", 
		30 => "0000000000000000", 
		31 => "0000000000000000" 
	);


begin

	process (iCLK, iRST) begin
		if(iRST = '1') then
			ram <= cram;
		elsif(falling_edge(iCLK)) then
			if(iWE = '1') then
				case(iA) is
					when "00000" => ram(0) <= iD;
					when "00001" => ram(1) <= iD;
					when "00010" => ram(2) <= iD;
					when "00011" => ram(3) <= iD;
					when "00100" => ram(4) <= iD;
					when "00101" => ram(5) <= iD;
					when "00110" => ram(6) <= iD;
					when "00111" => ram(7) <= iD;
					when "01000" => ram(8) <= iD;
					when "01001" => ram(9) <= iD;
					when "01010" => ram(10) <= iD;
					when "01011" => ram(11) <= iD;
					when "01100" => ram(12) <= iD;
					when "01101" => ram(13) <= iD;
					when "01110" => ram(14) <= iD;
					when "01111" => ram(15) <= iD;
					when "10000" => ram(16) <= iD;
					when "10001" => ram(17) <= iD;
					when "10010" => ram(18) <= iD;
					when "10011" => ram(19) <= iD;
					when "10100" => ram(20) <= iD;
					when "10101" => ram(21) <= iD;
					when "10110" => ram(22) <= iD;
					when "10111" => ram(23) <= iD;
					when "11000" => ram(24) <= iD;
					when "11001" => ram(25) <= iD;
					when "11010" => ram(26) <= iD;
					when "11011" => ram(27) <= iD;
					when "11100" => ram(28) <= iD;
					when "11101" => ram(29) <= iD;
					when "11110" => ram(30) <= iD;
					when "11111" => ram(31) <= iD;
					when others => ram <= cram;				
				end case;
			else
				ram <= ram;
			end if;
		end if;
	end process;

	process (iA) begin 
		case(iA) is
			when "00000" => oQ <= ram(0);
			when "00001" => oQ <= ram(1);
			when "00010" => oQ <= ram(2);
			when "00011" => oQ <= ram(3);
			when "00100" => oQ <= ram(4);
			when "00101" => oQ <= ram(5);
			when "00110" => oQ <= ram(6);
			when "00111" => oQ <= ram(7);
			when "01000" => oQ <= ram(8);
			when "01001" => oQ <= ram(9);
			when "01010" => oQ <= ram(10);
			when "01011" => oQ <= ram(11);
			when "01100" => oQ <= ram(12);
			when "01101" => oQ <= ram(13);
			when "01110" => oQ <= ram(14);
			when "01111" => oQ <= ram(15);
			when "10000" => oQ <= ram(16);
			when "10001" => oQ <= ram(17);
			when "10010" => oQ <= ram(18);
			when "10011" => oQ <= ram(19);
			when "10100" => oQ <= ram(20);
			when "10101" => oQ <= ram(21);
			when "10110" => oQ <= ram(22);
			when "10111" => oQ <= ram(23);
			when "11000" => oQ <= ram(24);
			when "11001" => oQ <= ram(25);
			when "11010" => oQ <= ram(26);
			when "11011" => oQ <= ram(27);
			when "11100" => oQ <= ram(28);
			when "11101" => oQ <= ram(29);
			when "11110" => oQ <= ram(30);
			when "11111" => oQ <= ram(31);
			when others => oQ <= (others => '0');
		end case;
	end process;


end architecture;