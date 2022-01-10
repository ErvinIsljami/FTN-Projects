library ieee;
use ieee.std_logic_1164.all;

entity cpu_top_tb is
end entity;

architecture Behavioral of cpu_top_tb is

	component cpu_top is
		port (
			iCLK	:  in std_logic;
			iRST	:  in std_logic;
			iDATA	:	in std_logic_vector(15 downto 0);
			oDATA	: out std_logic_vector(15 downto 0)
		);
	end component;
	
	signal sCLK		: std_logic;
	signal sRST		: std_logic	:= '0';
	signal sIDATA	: std_logic_vector(15 downto 0) := "0000000000000101";
	signal sODATA	: std_logic_vector(15 downto 0);
	
	constant iCLK_period : time := 1 ns;

begin

	uut : cpu_top port map (
		iCLK	=> sCLK,
		iRST	=> sRST,
		iDATA	=> sIDATA,
		oDATA	=> sODATA
	);

	iCLK_process : process
	begin
		sCLK <= '0';
		wait for iCLK_period / 2;
		sCLK <= '1';
		wait for iCLK_period / 2;
	end process;
	
	stimulus : process
	begin
		sRST <= '1';
		wait for 10 * iCLK_period;
		sRST <= '0';
		
		
		wait;
	end process;
end architecture;