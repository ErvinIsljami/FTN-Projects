library ieee;
use ieee.std_logic_1164.all;

entity instr_rom is
	port (
		iA		:	in std_logic_vector(4 downto 0);
		oQ		: out std_logic_vector(14 downto 0)
	);
end entity;

architecture Behavioral of instr_rom is

	type rom_array is array (0 to 31) of std_logic_vector (14 downto 0);
	
	constant rom : rom_array := (
		0 => "000000000000000", 
		1 => "000000000000000", 
		2 => "000000000000000", 
		3 => "000000000000000", 
		4 => "000000000000000", 
		5 => "000000000000000", 
		6 => "000000000000000", 
		7 => "000000000000000", 
		8 => "000000000000000", 
		9 => "000000000000000", 
		10 => "000000000000000", 
		11 => "000000000000000", 
		12 => "000000000000000", 
		13 => "000000000000000", 
		14 => "000000000000000", 
		15 => "000000000000000", 
		16 => "000000000000000", 
		17 => "000000000000000", 
		18 => "000000000000000", 
		19 => "000000000000000",
		20 => "000000000000000", 
		21 => "000000000000000", 
		22 => "000000000000000", 
		23 => "000000000000000", 
		24 => "000000000000000", 
		25 => "000000000000000", 
		26 => "000000000000000", 
		27 => "000000000000000", 
		28 => "000000000000000", 
		29 => "000000000000000", 
		30 => "000000000000000", 
		31 => "000000000000000" 
	);


begin

	process (iA) begin 
		case(iA) is
			when "00000" => oQ <= rom(0);
			when "00001" => oQ <= rom(1);
			when "00010" => oQ <= rom(2);
			when "00011" => oQ <= rom(3);
			when "00100" => oQ <= rom(4);
			when "00101" => oQ <= rom(5);
			when "00110" => oQ <= rom(6);
			when "00111" => oQ <= rom(7);
			when "01000" => oQ <= rom(8);
			when "01001" => oQ <= rom(9);
			when "01010" => oQ <= rom(10);
			when "01011" => oQ <= rom(11);
			when "01100" => oQ <= rom(12);
			when "01101" => oQ <= rom(13);
			when "01110" => oQ <= rom(14);
			when "01111" => oQ <= rom(15);
			when "10000" => oQ <= rom(16);
			when "10001" => oQ <= rom(17);
			when "10010" => oQ <= rom(18);
			when "10011" => oQ <= rom(19);
			when "10100" => oQ <= rom(20);
			when "10101" => oQ <= rom(21);
			when "10110" => oQ <= rom(22);
			when "10111" => oQ <= rom(23);
			when "11000" => oQ <= rom(24);
			when "11001" => oQ <= rom(25);
			when "11010" => oQ <= rom(26);
			when "11011" => oQ <= rom(27);
			when "11100" => oQ <= rom(28);
			when "11101" => oQ <= rom(29);
			when "11110" => oQ <= rom(30);
			when "11111" => oQ <= rom(31);
			when others => oQ <= (others => '0');
		end case;
	end process;


end architecture;