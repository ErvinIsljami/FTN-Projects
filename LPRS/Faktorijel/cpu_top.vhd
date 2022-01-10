library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

entity cpu_top is
	port (
		iCLK	:  in std_logic;
		iRST	:  in std_logic;
		iDATA	:	in std_logic_vector(15 downto 0);
		oDATA	: out std_logic_vector(15 downto 0)
	);
end entity;

architecture Behavioral of cpu_top is
	
	component Reg is
		port (
			iCLK	:  in std_logic;
			iRST	:  in std_logic;
			iD		:  in std_logic_vector(15 downto 0);
			iWE	:  in std_logic;
			oQ		: out std_logic_vector(15 downto 0)
		);
	end component;
	
	component mux is
		port (
			iD0	:	in std_logic_vector(15 downto 0);
			iD1	:	in std_logic_vector(15 downto 0);
			iD2	:	in std_logic_vector(15 downto 0);
			iD3	:	in std_logic_vector(15 downto 0);
			iD4	:	in std_logic_vector(15 downto 0);
			iD5	:	in std_logic_vector(15 downto 0);
			iD6	:	in std_logic_vector(15 downto 0);
			iD7	:	in std_logic_vector(15 downto 0);
			iD8	:	in std_logic_vector(15 downto 0);
			iSEL	:  in std_logic_vector(3 downto 0);
			oQ		: out std_logic_vector(15 downto 0)
		);
	end component;
	
	component alu is
		port (
			iA		:	in std_logic_vector(15 downto 0);
			iB		:	in std_logic_vector(15 downto 0);
			iSEL	:  in std_logic_vector(3 downto 0);
			oC		: out std_logic_vector(15 downto 0);
			oZERO	: out std_logic;
			oSIGN	: out std_logic;
			oCARRY: out std_logic
		);
	end component;
	
	component cu is
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
	end component;
	
	--nova komponenta
	component CntReg is
	port (
		iCLK	:  in std_logic;
		iRST	:  in std_logic;
		iEN	:  in std_logic;
		oQ		: out std_logic_vector(15 downto 0)
	);
	end component;
	
	-- signals
	signal sREG_WE 	: std_logic_vector(7 downto 0);
	signal sR0			: std_logic_vector(15 downto 0);
	signal sR1			: std_logic_vector(15 downto 0);
	signal sR2			: std_logic_vector(15 downto 0);
	signal sR3			: std_logic_vector(15 downto 0);
	signal sR4			: std_logic_vector(15 downto 0);
	signal sR5			: std_logic_vector(15 downto 0);
	signal sR6			: std_logic_vector(15 downto 0);
	signal sR7			: std_logic_vector(15 downto 0);
	
	signal sMUXA		: std_logic_vector(15 downto 0);
	signal sMUXA_SEL	: std_logic_vector(3 downto 0);
	signal sMUXB		: std_logic_vector(15 downto 0);
	signal sMUXB_SEL	: std_logic_vector(3 downto 0);

	signal sALU_SEL	: std_logic_vector(3 downto 0);
	signal sRESULT		: std_logic_vector(15 downto 0);
	
	signal sZERO		: std_logic;
	signal sSIGN		: std_logic;
	signal sCARRY		: std_logic;
	--novi signali
	signal sCNT_EN : std_logic;
	signal sCNT		: std_logic_vector(15 downto 0);
	
begin

	R0 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(0),
		oQ		=> sR0
	);
	
	R1 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(1),
		oQ		=> sR1
	);
	
	R2 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(2),
		oQ		=> sR2
	);
	
	R3 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(3),
		oQ		=> sR3
	);
	
	R4 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(4),
		oQ		=> sR4
	);
	
	R5 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(5),
		oQ		=> sR5
	);
	
	R6 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(6),
		oQ		=> sR6
	);
	
	R7 : reg port map (
		iCLK	=>	iCLK,
		iRST	=> iRST,
		iD		=> sRESULT, 
		iWE	=> sREG_WE(7),
		oQ		=> sR7
	);
	
	MUXA : mux port map (
		iD0	=> sR0,
		iD1	=> sR1,
		iD2	=> sR2,
		iD3	=> sR3,
		iD4	=> sR4,
		iD5	=> sR5,
		iD6	=> sR6,
		iD7	=> sR7,
		iD8	=> iDATA,
		iSEL	=> sMUXA_SEL,
		oQ		=> sMUXA
	);
	
	MUXB : mux port map (
		iD0	=> sR0,
		iD1	=> sR1,
		iD2	=> sR2,
		iD3	=> sR3,
		iD4	=> sR4,
		iD5	=> sR5,
		iD6	=> sR6,
		iD7	=> sR7,
		iD8	=> iDATA,
		iSEL	=> sMUXB_SEL,
		oQ		=> sMUXB
	);
	
	iALU : alu port map (
		iA		=> sMUXA,
		iB		=> sMUXB,
		iSEL	=> sALU_SEL,
		oC		=> sRESULT,
		oZERO	=> sZERO,
		oSIGN	=> sSIGN,
		oCARRY=> sCARRY
	);
	
	iCU : cu port map (
		iCLK		=> iCLK,
		iRST		=> iRST,
		iZERO		=> sZERO,
		iSIGN		=> sSIGN,
		iCARRY	=> sCARRY,
		oREG_WE	=> sREG_WE,
		oMUXA_SEL=> sMUXA_SEL,
		oMUXB_SEL=> sMUXB_SEL,
		oALU_SEL	=> sALU_SEL
	);
	
	--nova komponenta
	sCNT_EN <= '1' when sALU_SEL = "0001" or sALU_SEL = "0010" or sALU_SEL = "0110" or sALU_SEL = "0111" else '0'; 
	
	iCNT : cntReg port map (
		iCLK	=> iCLK,
		iRST	=> iRST,
		iEN	=> sCNT_EN,
		oQ		=> sCNT
	);
	oDATA <= sRESULT;
	
end architecture;