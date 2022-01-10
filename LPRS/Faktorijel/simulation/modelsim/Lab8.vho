-- Copyright (C) 2018  Intel Corporation. All rights reserved.
-- Your use of Intel Corporation's design tools, logic functions 
-- and other software and tools, and its AMPP partner logic 
-- functions, and any output files from any of the foregoing 
-- (including device programming or simulation files), and any 
-- associated documentation or information are expressly subject 
-- to the terms and conditions of the Intel Program License 
-- Subscription Agreement, the Intel Quartus Prime License Agreement,
-- the Intel FPGA IP License Agreement, or other applicable license
-- agreement, including, without limitation, that your use is for
-- the sole purpose of programming logic devices manufactured by
-- Intel and sold by Intel or its authorized distributors.  Please
-- refer to the applicable agreement for further details.

-- VENDOR "Altera"
-- PROGRAM "Quartus Prime"
-- VERSION "Version 18.0.0 Build 614 04/24/2018 SJ Lite Edition"

-- DATE "01/30/2020 12:36:04"

-- 
-- Device: Altera 10M16SAU169C8G Package UFBGA169
-- 

-- 
-- This VHDL file should be used for ModelSim-Altera (VHDL) only
-- 

LIBRARY FIFTYFIVENM;
LIBRARY IEEE;
USE FIFTYFIVENM.FIFTYFIVENM_COMPONENTS.ALL;
USE IEEE.STD_LOGIC_1164.ALL;

ENTITY 	hard_block IS
    PORT (
	devoe : IN std_logic;
	devclrn : IN std_logic;
	devpor : IN std_logic
	);
END hard_block;

-- Design Ports Information
-- ~ALTERA_TMS~	=>  Location: PIN_G1,	 I/O Standard: 2.5 V Schmitt Trigger,	 Current Strength: Default
-- ~ALTERA_TCK~	=>  Location: PIN_G2,	 I/O Standard: 2.5 V Schmitt Trigger,	 Current Strength: Default
-- ~ALTERA_TDI~	=>  Location: PIN_F5,	 I/O Standard: 2.5 V Schmitt Trigger,	 Current Strength: Default
-- ~ALTERA_TDO~	=>  Location: PIN_F6,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- ~ALTERA_CONFIG_SEL~	=>  Location: PIN_D7,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- ~ALTERA_nCONFIG~	=>  Location: PIN_E7,	 I/O Standard: 2.5 V Schmitt Trigger,	 Current Strength: Default
-- ~ALTERA_nSTATUS~	=>  Location: PIN_C4,	 I/O Standard: 2.5 V Schmitt Trigger,	 Current Strength: Default
-- ~ALTERA_CONF_DONE~	=>  Location: PIN_C5,	 I/O Standard: 2.5 V Schmitt Trigger,	 Current Strength: Default


ARCHITECTURE structure OF hard_block IS
SIGNAL gnd : std_logic := '0';
SIGNAL vcc : std_logic := '1';
SIGNAL unknown : std_logic := 'X';
SIGNAL ww_devoe : std_logic;
SIGNAL ww_devclrn : std_logic;
SIGNAL ww_devpor : std_logic;
SIGNAL \~ALTERA_TMS~~padout\ : std_logic;
SIGNAL \~ALTERA_TCK~~padout\ : std_logic;
SIGNAL \~ALTERA_TDI~~padout\ : std_logic;
SIGNAL \~ALTERA_CONFIG_SEL~~padout\ : std_logic;
SIGNAL \~ALTERA_nCONFIG~~padout\ : std_logic;
SIGNAL \~ALTERA_nSTATUS~~padout\ : std_logic;
SIGNAL \~ALTERA_CONF_DONE~~padout\ : std_logic;
SIGNAL \~ALTERA_TMS~~ibuf_o\ : std_logic;
SIGNAL \~ALTERA_TCK~~ibuf_o\ : std_logic;
SIGNAL \~ALTERA_TDI~~ibuf_o\ : std_logic;
SIGNAL \~ALTERA_CONFIG_SEL~~ibuf_o\ : std_logic;
SIGNAL \~ALTERA_nCONFIG~~ibuf_o\ : std_logic;
SIGNAL \~ALTERA_nSTATUS~~ibuf_o\ : std_logic;
SIGNAL \~ALTERA_CONF_DONE~~ibuf_o\ : std_logic;

BEGIN

ww_devoe <= devoe;
ww_devclrn <= devclrn;
ww_devpor <= devpor;
END structure;


LIBRARY ALTERA;
LIBRARY FIFTYFIVENM;
LIBRARY IEEE;
USE ALTERA.ALTERA_PRIMITIVES_COMPONENTS.ALL;
USE FIFTYFIVENM.FIFTYFIVENM_COMPONENTS.ALL;
USE IEEE.STD_LOGIC_1164.ALL;

ENTITY 	cpu_top IS
    PORT (
	iCLK : IN std_logic;
	iRST : IN std_logic;
	iDATA : IN std_logic_vector(15 DOWNTO 0);
	oDATA : BUFFER std_logic_vector(15 DOWNTO 0)
	);
END cpu_top;

-- Design Ports Information
-- oDATA[0]	=>  Location: PIN_C9,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[1]	=>  Location: PIN_F13,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[2]	=>  Location: PIN_A9,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[3]	=>  Location: PIN_C13,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[4]	=>  Location: PIN_F12,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[5]	=>  Location: PIN_B9,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[6]	=>  Location: PIN_G10,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[7]	=>  Location: PIN_E1,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[8]	=>  Location: PIN_B10,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[9]	=>  Location: PIN_C1,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[10]	=>  Location: PIN_E3,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[11]	=>  Location: PIN_B7,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[12]	=>  Location: PIN_D8,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[13]	=>  Location: PIN_C10,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[14]	=>  Location: PIN_G9,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- oDATA[15]	=>  Location: PIN_A11,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[0]	=>  Location: PIN_A10,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[1]	=>  Location: PIN_A8,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[2]	=>  Location: PIN_A7,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[3]	=>  Location: PIN_A3,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[4]	=>  Location: PIN_K8,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[5]	=>  Location: PIN_F9,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[6]	=>  Location: PIN_E13,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[7]	=>  Location: PIN_E4,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[8]	=>  Location: PIN_E8,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[9]	=>  Location: PIN_E9,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[10]	=>  Location: PIN_C2,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[11]	=>  Location: PIN_F1,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[12]	=>  Location: PIN_D1,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[13]	=>  Location: PIN_B1,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[14]	=>  Location: PIN_E12,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iDATA[15]	=>  Location: PIN_A6,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iCLK	=>  Location: PIN_H5,	 I/O Standard: 2.5 V,	 Current Strength: Default
-- iRST	=>  Location: PIN_H4,	 I/O Standard: 2.5 V,	 Current Strength: Default


ARCHITECTURE structure OF cpu_top IS
SIGNAL gnd : std_logic := '0';
SIGNAL vcc : std_logic := '1';
SIGNAL unknown : std_logic := 'X';
SIGNAL devoe : std_logic := '1';
SIGNAL devclrn : std_logic := '1';
SIGNAL devpor : std_logic := '1';
SIGNAL ww_devoe : std_logic;
SIGNAL ww_devclrn : std_logic;
SIGNAL ww_devpor : std_logic;
SIGNAL ww_iCLK : std_logic;
SIGNAL ww_iRST : std_logic;
SIGNAL ww_iDATA : std_logic_vector(15 DOWNTO 0);
SIGNAL ww_oDATA : std_logic_vector(15 DOWNTO 0);
SIGNAL \~QUARTUS_CREATED_ADC1~_CHSEL_bus\ : std_logic_vector(4 DOWNTO 0);
SIGNAL \iRST~inputclkctrl_INCLK_bus\ : std_logic_vector(3 DOWNTO 0);
SIGNAL \iCLK~inputclkctrl_INCLK_bus\ : std_logic_vector(3 DOWNTO 0);
SIGNAL \~QUARTUS_CREATED_GND~I_combout\ : std_logic;
SIGNAL \~QUARTUS_CREATED_UNVM~~busy\ : std_logic;
SIGNAL \~QUARTUS_CREATED_ADC1~~eoc\ : std_logic;
SIGNAL \oDATA[0]~output_o\ : std_logic;
SIGNAL \oDATA[1]~output_o\ : std_logic;
SIGNAL \oDATA[2]~output_o\ : std_logic;
SIGNAL \oDATA[3]~output_o\ : std_logic;
SIGNAL \oDATA[4]~output_o\ : std_logic;
SIGNAL \oDATA[5]~output_o\ : std_logic;
SIGNAL \oDATA[6]~output_o\ : std_logic;
SIGNAL \oDATA[7]~output_o\ : std_logic;
SIGNAL \oDATA[8]~output_o\ : std_logic;
SIGNAL \oDATA[9]~output_o\ : std_logic;
SIGNAL \oDATA[10]~output_o\ : std_logic;
SIGNAL \oDATA[11]~output_o\ : std_logic;
SIGNAL \oDATA[12]~output_o\ : std_logic;
SIGNAL \oDATA[13]~output_o\ : std_logic;
SIGNAL \oDATA[14]~output_o\ : std_logic;
SIGNAL \oDATA[15]~output_o\ : std_logic;
SIGNAL \iCLK~input_o\ : std_logic;
SIGNAL \iCLK~inputclkctrl_outclk\ : std_logic;
SIGNAL \iCU|currentState.START~feeder_combout\ : std_logic;
SIGNAL \iRST~input_o\ : std_logic;
SIGNAL \iRST~inputclkctrl_outclk\ : std_logic;
SIGNAL \iCU|currentState.START~q\ : std_logic;
SIGNAL \iCU|currentState.T1~0_combout\ : std_logic;
SIGNAL \iCU|currentState.T1~q\ : std_logic;
SIGNAL \iCU|currentState.T2~q\ : std_logic;
SIGNAL \iDATA[15]~input_o\ : std_logic;
SIGNAL \iCU|oREG_WE[1]~0_combout\ : std_logic;
SIGNAL \iCU|WideOr3~combout\ : std_logic;
SIGNAL \iCU|WideOr2~combout\ : std_logic;
SIGNAL \MUXB|oQ[1]~13_combout\ : std_logic;
SIGNAL \iCU|WideOr5~1_combout\ : std_logic;
SIGNAL \MUXB|oQ[1]~14_combout\ : std_logic;
SIGNAL \MUXB|oQ[0]~15_combout\ : std_logic;
SIGNAL \MUXB|oQ[0]~16_combout\ : std_logic;
SIGNAL \iALU|Add0~1_combout\ : std_logic;
SIGNAL \iCU|Selector0~7_combout\ : std_logic;
SIGNAL \iCU|WideOr6~combout\ : std_logic;
SIGNAL \MUXB|oQ[15]~45_combout\ : std_logic;
SIGNAL \MUXB|oQ[15]~46_combout\ : std_logic;
SIGNAL \iALU|Add0~78_combout\ : std_logic;
SIGNAL \iDATA[14]~input_o\ : std_logic;
SIGNAL \iALU|Mux12~1_combout\ : std_logic;
SIGNAL \MUXB|oQ[14]~43_combout\ : std_logic;
SIGNAL \MUXB|oQ[14]~44_combout\ : std_logic;
SIGNAL \iALU|Add0~76_combout\ : std_logic;
SIGNAL \iALU|Add0~73_combout\ : std_logic;
SIGNAL \iDATA[13]~input_o\ : std_logic;
SIGNAL \R2|sREG[13]~feeder_combout\ : std_logic;
SIGNAL \R0|sREG[13]~feeder_combout\ : std_logic;
SIGNAL \MUXB|oQ[13]~41_combout\ : std_logic;
SIGNAL \MUXB|oQ[13]~42_combout\ : std_logic;
SIGNAL \iALU|Add0~71_combout\ : std_logic;
SIGNAL \iALU|Add0~68_combout\ : std_logic;
SIGNAL \iDATA[12]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[12]~38_combout\ : std_logic;
SIGNAL \MUXA|oQ[12]~39_combout\ : std_logic;
SIGNAL \MUXA|oQ[12]~40_combout\ : std_logic;
SIGNAL \iALU|Add0~66_combout\ : std_logic;
SIGNAL \iDATA[11]~input_o\ : std_logic;
SIGNAL \MUXB|oQ[11]~37_combout\ : std_logic;
SIGNAL \MUXB|oQ[11]~38_combout\ : std_logic;
SIGNAL \iALU|Add0~61_combout\ : std_logic;
SIGNAL \iALU|Add0~58_combout\ : std_logic;
SIGNAL \iDATA[10]~input_o\ : std_logic;
SIGNAL \MUXB|oQ[10]~35_combout\ : std_logic;
SIGNAL \MUXB|oQ[10]~36_combout\ : std_logic;
SIGNAL \iALU|Add0~56_combout\ : std_logic;
SIGNAL \iALU|Add0~53_combout\ : std_logic;
SIGNAL \iDATA[9]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[9]~29_combout\ : std_logic;
SIGNAL \MUXA|oQ[9]~30_combout\ : std_logic;
SIGNAL \MUXA|oQ[9]~31_combout\ : std_logic;
SIGNAL \iDATA[8]~input_o\ : std_logic;
SIGNAL \MUXB|oQ[8]~31_combout\ : std_logic;
SIGNAL \MUXB|oQ[8]~32_combout\ : std_logic;
SIGNAL \iALU|Add0~46_combout\ : std_logic;
SIGNAL \iALU|Add0~43_combout\ : std_logic;
SIGNAL \iDATA[7]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[7]~23_combout\ : std_logic;
SIGNAL \MUXA|oQ[7]~24_combout\ : std_logic;
SIGNAL \MUXA|oQ[7]~25_combout\ : std_logic;
SIGNAL \iDATA[6]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[6]~20_combout\ : std_logic;
SIGNAL \MUXA|oQ[6]~21_combout\ : std_logic;
SIGNAL \MUXA|oQ[6]~22_combout\ : std_logic;
SIGNAL \iALU|Add0~36_combout\ : std_logic;
SIGNAL \iDATA[5]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[5]~17_combout\ : std_logic;
SIGNAL \MUXA|oQ[5]~18_combout\ : std_logic;
SIGNAL \MUXA|oQ[5]~19_combout\ : std_logic;
SIGNAL \iDATA[4]~input_o\ : std_logic;
SIGNAL \R2|sREG[4]~feeder_combout\ : std_logic;
SIGNAL \MUXA|oQ[4]~14_combout\ : std_logic;
SIGNAL \MUXA|oQ[4]~15_combout\ : std_logic;
SIGNAL \MUXA|oQ[4]~16_combout\ : std_logic;
SIGNAL \iALU|Add0~26_combout\ : std_logic;
SIGNAL \iDATA[3]~input_o\ : std_logic;
SIGNAL \R1|sREG[3]~feeder_combout\ : std_logic;
SIGNAL \MUXA|oQ[3]~11_combout\ : std_logic;
SIGNAL \MUXA|oQ[3]~12_combout\ : std_logic;
SIGNAL \MUXA|oQ[3]~13_combout\ : std_logic;
SIGNAL \iALU|Add0~21_combout\ : std_logic;
SIGNAL \iDATA[2]~input_o\ : std_logic;
SIGNAL \MUXB|oQ[2]~19_combout\ : std_logic;
SIGNAL \MUXB|oQ[2]~20_combout\ : std_logic;
SIGNAL \iALU|Add0~16_combout\ : std_logic;
SIGNAL \iALU|Add0~13_combout\ : std_logic;
SIGNAL \iDATA[1]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[1]~5_combout\ : std_logic;
SIGNAL \MUXA|oQ[1]~6_combout\ : std_logic;
SIGNAL \MUXA|oQ[1]~7_combout\ : std_logic;
SIGNAL \iALU|Add0~11_combout\ : std_logic;
SIGNAL \iDATA[0]~input_o\ : std_logic;
SIGNAL \MUXA|oQ[0]~1_combout\ : std_logic;
SIGNAL \MUXA|oQ[0]~2_combout\ : std_logic;
SIGNAL \MUXA|oQ[0]~3_combout\ : std_logic;
SIGNAL \MUXA|oQ[0]~4_combout\ : std_logic;
SIGNAL \iALU|Add0~3_cout\ : std_logic;
SIGNAL \iALU|Add0~5\ : std_logic;
SIGNAL \iALU|Add0~9_combout\ : std_logic;
SIGNAL \iALU|Add0~12_combout\ : std_logic;
SIGNAL \MUXB|oQ[1]~17_combout\ : std_logic;
SIGNAL \MUXB|oQ[1]~18_combout\ : std_logic;
SIGNAL \iALU|Add0~8_combout\ : std_logic;
SIGNAL \iALU|Add0~10\ : std_logic;
SIGNAL \iALU|Add0~14_combout\ : std_logic;
SIGNAL \iALU|Add0~17_combout\ : std_logic;
SIGNAL \MUXA|oQ[2]~8_combout\ : std_logic;
SIGNAL \MUXA|oQ[2]~9_combout\ : std_logic;
SIGNAL \MUXA|oQ[2]~10_combout\ : std_logic;
SIGNAL \iALU|Add0~15\ : std_logic;
SIGNAL \iALU|Add0~19_combout\ : std_logic;
SIGNAL \iALU|Add0~22_combout\ : std_logic;
SIGNAL \R2|sREG[3]~feeder_combout\ : std_logic;
SIGNAL \MUXB|oQ[3]~21_combout\ : std_logic;
SIGNAL \MUXB|oQ[3]~22_combout\ : std_logic;
SIGNAL \iALU|Add0~18_combout\ : std_logic;
SIGNAL \iALU|Add0~20\ : std_logic;
SIGNAL \iALU|Add0~24_combout\ : std_logic;
SIGNAL \iALU|Add0~27_combout\ : std_logic;
SIGNAL \MUXB|oQ[4]~23_combout\ : std_logic;
SIGNAL \MUXB|oQ[4]~24_combout\ : std_logic;
SIGNAL \iALU|Add0~23_combout\ : std_logic;
SIGNAL \iALU|Add0~25\ : std_logic;
SIGNAL \iALU|Add0~29_combout\ : std_logic;
SIGNAL \iALU|Add0~31_combout\ : std_logic;
SIGNAL \iALU|Add0~32_combout\ : std_logic;
SIGNAL \MUXB|oQ[5]~25_combout\ : std_logic;
SIGNAL \MUXB|oQ[5]~26_combout\ : std_logic;
SIGNAL \iALU|Add0~28_combout\ : std_logic;
SIGNAL \iALU|Add0~30\ : std_logic;
SIGNAL \iALU|Add0~34_combout\ : std_logic;
SIGNAL \iALU|Add0~37_combout\ : std_logic;
SIGNAL \MUXB|oQ[6]~27_combout\ : std_logic;
SIGNAL \MUXB|oQ[6]~28_combout\ : std_logic;
SIGNAL \iALU|Add0~33_combout\ : std_logic;
SIGNAL \iALU|Add0~35\ : std_logic;
SIGNAL \iALU|Add0~39_combout\ : std_logic;
SIGNAL \iALU|Add0~41_combout\ : std_logic;
SIGNAL \iALU|Add0~42_combout\ : std_logic;
SIGNAL \MUXB|oQ[7]~29_combout\ : std_logic;
SIGNAL \MUXB|oQ[7]~30_combout\ : std_logic;
SIGNAL \iALU|Add0~38_combout\ : std_logic;
SIGNAL \iALU|Add0~40\ : std_logic;
SIGNAL \iALU|Add0~44_combout\ : std_logic;
SIGNAL \iALU|Add0~47_combout\ : std_logic;
SIGNAL \MUXA|oQ[8]~26_combout\ : std_logic;
SIGNAL \MUXA|oQ[8]~27_combout\ : std_logic;
SIGNAL \MUXA|oQ[8]~28_combout\ : std_logic;
SIGNAL \iALU|Add0~45\ : std_logic;
SIGNAL \iALU|Add0~49_combout\ : std_logic;
SIGNAL \iALU|Add0~51_combout\ : std_logic;
SIGNAL \iALU|Add0~52_combout\ : std_logic;
SIGNAL \MUXB|oQ[9]~33_combout\ : std_logic;
SIGNAL \MUXB|oQ[9]~34_combout\ : std_logic;
SIGNAL \iALU|Add0~48_combout\ : std_logic;
SIGNAL \iALU|Add0~50\ : std_logic;
SIGNAL \iALU|Add0~54_combout\ : std_logic;
SIGNAL \iALU|Add0~57_combout\ : std_logic;
SIGNAL \MUXA|oQ[10]~32_combout\ : std_logic;
SIGNAL \MUXA|oQ[10]~33_combout\ : std_logic;
SIGNAL \MUXA|oQ[10]~34_combout\ : std_logic;
SIGNAL \iALU|Add0~55\ : std_logic;
SIGNAL \iALU|Add0~59_combout\ : std_logic;
SIGNAL \iALU|Add0~62_combout\ : std_logic;
SIGNAL \MUXA|oQ[11]~35_combout\ : std_logic;
SIGNAL \MUXA|oQ[11]~36_combout\ : std_logic;
SIGNAL \MUXA|oQ[11]~37_combout\ : std_logic;
SIGNAL \iALU|Add0~60\ : std_logic;
SIGNAL \iALU|Add0~64_combout\ : std_logic;
SIGNAL \iALU|Add0~67_combout\ : std_logic;
SIGNAL \MUXB|oQ[12]~39_combout\ : std_logic;
SIGNAL \MUXB|oQ[12]~40_combout\ : std_logic;
SIGNAL \iALU|Add0~63_combout\ : std_logic;
SIGNAL \iALU|Add0~65\ : std_logic;
SIGNAL \iALU|Add0~69_combout\ : std_logic;
SIGNAL \iALU|Add0~72_combout\ : std_logic;
SIGNAL \MUXA|oQ[13]~41_combout\ : std_logic;
SIGNAL \MUXA|oQ[13]~42_combout\ : std_logic;
SIGNAL \MUXA|oQ[13]~43_combout\ : std_logic;
SIGNAL \iALU|Add0~70\ : std_logic;
SIGNAL \iALU|Add0~74_combout\ : std_logic;
SIGNAL \iALU|Add0~77_combout\ : std_logic;
SIGNAL \MUXA|oQ[14]~44_combout\ : std_logic;
SIGNAL \MUXA|oQ[14]~45_combout\ : std_logic;
SIGNAL \MUXA|oQ[14]~46_combout\ : std_logic;
SIGNAL \iALU|Add0~75\ : std_logic;
SIGNAL \iALU|Add0~79_combout\ : std_logic;
SIGNAL \iCU|Selector0~8_combout\ : std_logic;
SIGNAL \iCU|Selector0~5_combout\ : std_logic;
SIGNAL \iCU|Selector0~2_combout\ : std_logic;
SIGNAL \iCU|Selector0~4_combout\ : std_logic;
SIGNAL \iCU|Selector0~3_combout\ : std_logic;
SIGNAL \iCU|Selector0~6_combout\ : std_logic;
SIGNAL \iCU|currentState.DONE~0_combout\ : std_logic;
SIGNAL \iCU|currentState.DONE~q\ : std_logic;
SIGNAL \iCU|WideOr5~0_combout\ : std_logic;
SIGNAL \iCU|WideOr5~combout\ : std_logic;
SIGNAL \iALU|Mux12~0_combout\ : std_logic;
SIGNAL \iALU|Add0~82_combout\ : std_logic;
SIGNAL \MUXA|oQ[15]~47_combout\ : std_logic;
SIGNAL \MUXA|oQ[15]~48_combout\ : std_logic;
SIGNAL \MUXA|oQ[15]~49_combout\ : std_logic;
SIGNAL \iALU|Add0~81_combout\ : std_logic;
SIGNAL \iCU|nextState.T8~0_combout\ : std_logic;
SIGNAL \iCU|currentState.T8~q\ : std_logic;
SIGNAL \iCU|currentState.T9~q\ : std_logic;
SIGNAL \iCU|currentState.T10~feeder_combout\ : std_logic;
SIGNAL \iCU|currentState.T10~q\ : std_logic;
SIGNAL \iCU|nextState.T3~combout\ : std_logic;
SIGNAL \iCU|currentState.T3~q\ : std_logic;
SIGNAL \iCU|nextState.T4~0_combout\ : std_logic;
SIGNAL \iCU|currentState.T4~q\ : std_logic;
SIGNAL \iCU|nextState.T5~combout\ : std_logic;
SIGNAL \iCU|currentState.T5~q\ : std_logic;
SIGNAL \iCU|nextState.T6~0_combout\ : std_logic;
SIGNAL \iCU|currentState.T6~q\ : std_logic;
SIGNAL \iCU|currentState.T7~q\ : std_logic;
SIGNAL \iCU|WideOr4~combout\ : std_logic;
SIGNAL \iALU|Add0~6_combout\ : std_logic;
SIGNAL \iALU|Add0~4_combout\ : std_logic;
SIGNAL \iALU|Add0~7_combout\ : std_logic;
SIGNAL \R1|sREG\ : std_logic_vector(15 DOWNTO 0);
SIGNAL \R2|sREG\ : std_logic_vector(15 DOWNTO 0);
SIGNAL \R0|sREG\ : std_logic_vector(15 DOWNTO 0);
SIGNAL \iCU|oREG_WE\ : std_logic_vector(7 DOWNTO 0);
SIGNAL \R3|sREG\ : std_logic_vector(15 DOWNTO 0);
SIGNAL \ALT_INV_iRST~inputclkctrl_outclk\ : std_logic;

COMPONENT hard_block
    PORT (
	devoe : IN std_logic;
	devclrn : IN std_logic;
	devpor : IN std_logic);
END COMPONENT;

BEGIN

ww_iCLK <= iCLK;
ww_iRST <= iRST;
ww_iDATA <= iDATA;
oDATA <= ww_oDATA;
ww_devoe <= devoe;
ww_devclrn <= devclrn;
ww_devpor <= devpor;

\~QUARTUS_CREATED_ADC1~_CHSEL_bus\ <= (\~QUARTUS_CREATED_GND~I_combout\ & \~QUARTUS_CREATED_GND~I_combout\ & \~QUARTUS_CREATED_GND~I_combout\ & \~QUARTUS_CREATED_GND~I_combout\ & \~QUARTUS_CREATED_GND~I_combout\);

\iRST~inputclkctrl_INCLK_bus\ <= (vcc & vcc & vcc & \iRST~input_o\);

\iCLK~inputclkctrl_INCLK_bus\ <= (vcc & vcc & vcc & \iCLK~input_o\);
\ALT_INV_iRST~inputclkctrl_outclk\ <= NOT \iRST~inputclkctrl_outclk\;
auto_generated_inst : hard_block
PORT MAP (
	devoe => ww_devoe,
	devclrn => ww_devclrn,
	devpor => ww_devpor);

-- Location: LCCOMB_X26_Y19_N12
\~QUARTUS_CREATED_GND~I\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \~QUARTUS_CREATED_GND~I_combout\ = GND

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	combout => \~QUARTUS_CREATED_GND~I_combout\);

-- Location: IOOBUF_X19_Y17_N2
\oDATA[0]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~7_combout\,
	devoe => ww_devoe,
	o => \oDATA[0]~output_o\);

-- Location: IOOBUF_X50_Y15_N23
\oDATA[1]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~12_combout\,
	devoe => ww_devoe,
	o => \oDATA[1]~output_o\);

-- Location: IOOBUF_X19_Y17_N16
\oDATA[2]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~17_combout\,
	devoe => ww_devoe,
	o => \oDATA[2]~output_o\);

-- Location: IOOBUF_X50_Y21_N2
\oDATA[3]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~22_combout\,
	devoe => ww_devoe,
	o => \oDATA[3]~output_o\);

-- Location: IOOBUF_X50_Y16_N23
\oDATA[4]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~27_combout\,
	devoe => ww_devoe,
	o => \oDATA[4]~output_o\);

-- Location: IOOBUF_X19_Y17_N30
\oDATA[5]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~32_combout\,
	devoe => ww_devoe,
	o => \oDATA[5]~output_o\);

-- Location: IOOBUF_X50_Y14_N16
\oDATA[6]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~37_combout\,
	devoe => ww_devoe,
	o => \oDATA[6]~output_o\);

-- Location: IOOBUF_X25_Y22_N23
\oDATA[7]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~42_combout\,
	devoe => ww_devoe,
	o => \oDATA[7]~output_o\);

-- Location: IOOBUF_X19_Y17_N23
\oDATA[8]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~47_combout\,
	devoe => ww_devoe,
	o => \oDATA[8]~output_o\);

-- Location: IOOBUF_X25_Y23_N16
\oDATA[9]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~52_combout\,
	devoe => ww_devoe,
	o => \oDATA[9]~output_o\);

-- Location: IOOBUF_X25_Y24_N16
\oDATA[10]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~57_combout\,
	devoe => ww_devoe,
	o => \oDATA[10]~output_o\);

-- Location: IOOBUF_X14_Y17_N2
\oDATA[11]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~62_combout\,
	devoe => ww_devoe,
	o => \oDATA[11]~output_o\);

-- Location: IOOBUF_X16_Y17_N16
\oDATA[12]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~67_combout\,
	devoe => ww_devoe,
	o => \oDATA[12]~output_o\);

-- Location: IOOBUF_X21_Y17_N30
\oDATA[13]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~72_combout\,
	devoe => ww_devoe,
	o => \oDATA[13]~output_o\);

-- Location: IOOBUF_X50_Y14_N23
\oDATA[14]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~77_combout\,
	devoe => ww_devoe,
	o => \oDATA[14]~output_o\);

-- Location: IOOBUF_X16_Y17_N9
\oDATA[15]~output\ : fiftyfivenm_io_obuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	open_drain_output => "false")
-- pragma translate_on
PORT MAP (
	i => \iALU|Add0~82_combout\,
	devoe => ww_devoe,
	o => \oDATA[15]~output_o\);

-- Location: IOIBUF_X0_Y8_N15
\iCLK~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iCLK,
	o => \iCLK~input_o\);

-- Location: CLKCTRL_G3
\iCLK~inputclkctrl\ : fiftyfivenm_clkctrl
-- pragma translate_off
GENERIC MAP (
	clock_type => "global clock",
	ena_register_mode => "none")
-- pragma translate_on
PORT MAP (
	inclk => \iCLK~inputclkctrl_INCLK_bus\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	outclk => \iCLK~inputclkctrl_outclk\);

-- Location: LCCOMB_X30_Y17_N12
\iCU|currentState.START~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|currentState.START~feeder_combout\ = VCC

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111111111",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	combout => \iCU|currentState.START~feeder_combout\);

-- Location: IOIBUF_X0_Y8_N22
\iRST~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iRST,
	o => \iRST~input_o\);

-- Location: CLKCTRL_G4
\iRST~inputclkctrl\ : fiftyfivenm_clkctrl
-- pragma translate_off
GENERIC MAP (
	clock_type => "global clock",
	ena_register_mode => "none")
-- pragma translate_on
PORT MAP (
	inclk => \iRST~inputclkctrl_INCLK_bus\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	outclk => \iRST~inputclkctrl_outclk\);

-- Location: FF_X30_Y17_N13
\iCU|currentState.START\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|currentState.START~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.START~q\);

-- Location: LCCOMB_X30_Y17_N30
\iCU|currentState.T1~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|currentState.T1~0_combout\ = !\iCU|currentState.START~q\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000011111111",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iCU|currentState.START~q\,
	combout => \iCU|currentState.T1~0_combout\);

-- Location: FF_X30_Y17_N31
\iCU|currentState.T1\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|currentState.T1~0_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T1~q\);

-- Location: FF_X30_Y17_N5
\iCU|currentState.T2\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iCU|currentState.T1~q\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T2~q\);

-- Location: IOIBUF_X14_Y17_N29
\iDATA[15]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(15),
	o => \iDATA[15]~input_o\);

-- Location: LCCOMB_X33_Y17_N6
\iCU|oREG_WE[1]~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|oREG_WE[1]~0_combout\ = (\iCU|currentState.T7~q\) # (\iCU|currentState.T8~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111001100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T7~q\,
	datad => \iCU|currentState.T8~q\,
	combout => \iCU|oREG_WE[1]~0_combout\);

-- Location: LCCOMB_X33_Y17_N0
\iCU|WideOr3\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr3~combout\ = (\iCU|currentState.T9~q\) # ((\iCU|currentState.T5~q\) # ((\iCU|currentState.T3~q\) # (\iCU|oREG_WE[1]~0_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111111110",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T9~q\,
	datab => \iCU|currentState.T5~q\,
	datac => \iCU|currentState.T3~q\,
	datad => \iCU|oREG_WE[1]~0_combout\,
	combout => \iCU|WideOr3~combout\);

-- Location: LCCOMB_X30_Y17_N28
\iCU|WideOr2\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr2~combout\ = (\iCU|currentState.T3~q\) # ((\iCU|currentState.T2~q\) # (\iCU|currentState.T9~q\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111111100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T3~q\,
	datac => \iCU|currentState.T2~q\,
	datad => \iCU|currentState.T9~q\,
	combout => \iCU|WideOr2~combout\);

-- Location: FF_X34_Y17_N23
\R3|sREG[0]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~7_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(0));

-- Location: LCCOMB_X32_Y17_N0
\MUXB|oQ[1]~13\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[1]~13_combout\ = (!\iCU|currentState.T8~q\ & !\iCU|currentState.T5~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000001111",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datac => \iCU|currentState.T8~q\,
	datad => \iCU|currentState.T5~q\,
	combout => \MUXB|oQ[1]~13_combout\);

-- Location: FF_X34_Y17_N1
\R1|sREG[0]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~7_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(0));

-- Location: LCCOMB_X30_Y17_N4
\iCU|WideOr5~1\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr5~1_combout\ = (\iCU|currentState.T1~q\) # (\iCU|currentState.T10~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datac => \iCU|currentState.T1~q\,
	datad => \iCU|currentState.T10~q\,
	combout => \iCU|WideOr5~1_combout\);

-- Location: FF_X33_Y17_N13
\R2|sREG[0]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~7_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(0));

-- Location: LCCOMB_X32_Y17_N30
\iCU|oREG_WE[0]\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|oREG_WE\(0) = (\iCU|currentState.T6~q\) # (\iCU|currentState.T4~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111001100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T6~q\,
	datad => \iCU|currentState.T4~q\,
	combout => \iCU|oREG_WE\(0));

-- Location: FF_X32_Y17_N11
\R0|sREG[0]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~7_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(0));

-- Location: LCCOMB_X31_Y17_N30
\MUXB|oQ[1]~14\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[1]~14_combout\ = (\iCU|currentState.T6~q\) # (\iCU|currentState.T5~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datac => \iCU|currentState.T6~q\,
	datad => \iCU|currentState.T5~q\,
	combout => \MUXB|oQ[1]~14_combout\);

-- Location: LCCOMB_X32_Y17_N10
\MUXB|oQ[0]~15\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[0]~15_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(0))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(0)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(0),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(0),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[0]~15_combout\);

-- Location: LCCOMB_X34_Y17_N0
\MUXB|oQ[0]~16\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[0]~16_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[0]~15_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[0]~15_combout\ & (\R3|sREG\(0))) # (!\MUXB|oQ[0]~15_combout\ & ((\R1|sREG\(0))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001010000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R3|sREG\(0),
	datac => \R1|sREG\(0),
	datad => \MUXB|oQ[0]~15_combout\,
	combout => \MUXB|oQ[0]~16_combout\);

-- Location: LCCOMB_X34_Y17_N20
\iALU|Add0~1\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~1_combout\ = (\iCU|WideOr4~combout\) # (\iCU|WideOr5~combout\ $ (\MUXB|oQ[0]~16_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111111111100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[0]~16_combout\,
	combout => \iALU|Add0~1_combout\);

-- Location: LCCOMB_X34_Y16_N26
\iCU|Selector0~7\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~7_combout\ = (\iCU|currentState.T3~q\ & (\R3|sREG\(0) $ (((\iALU|Add0~1_combout\) # (!\iALU|Mux12~0_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0010001010000010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T3~q\,
	datab => \R3|sREG\(0),
	datac => \iALU|Mux12~0_combout\,
	datad => \iALU|Add0~1_combout\,
	combout => \iCU|Selector0~7_combout\);

-- Location: LCCOMB_X31_Y17_N8
\iCU|WideOr6\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr6~combout\ = (\iCU|currentState.T2~q\) # ((\iCU|currentState.T6~q\) # (\iCU|currentState.T9~q\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111101110",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T2~q\,
	datab => \iCU|currentState.T6~q\,
	datad => \iCU|currentState.T9~q\,
	combout => \iCU|WideOr6~combout\);

-- Location: FF_X35_Y17_N9
\R2|sREG[15]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~82_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(15));

-- Location: FF_X32_Y17_N31
\R0|sREG[15]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~82_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(15));

-- Location: FF_X34_Y17_N3
\R1|sREG[15]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~82_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(15));

-- Location: LCCOMB_X35_Y17_N10
\MUXB|oQ[15]~45\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[15]~45_combout\ = (\MUXB|oQ[1]~13_combout\ & (\R0|sREG\(15) & (!\MUXB|oQ[1]~14_combout\))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\) # (\R1|sREG\(15)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0101110101011000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R0|sREG\(15),
	datac => \MUXB|oQ[1]~14_combout\,
	datad => \R1|sREG\(15),
	combout => \MUXB|oQ[15]~45_combout\);

-- Location: LCCOMB_X35_Y17_N8
\MUXB|oQ[15]~46\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[15]~46_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[15]~45_combout\ & (\R3|sREG\(15))) # (!\MUXB|oQ[15]~45_combout\ & ((\R2|sREG\(15)))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[15]~45_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~14_combout\,
	datab => \R3|sREG\(15),
	datac => \R2|sREG\(15),
	datad => \MUXB|oQ[15]~45_combout\,
	combout => \MUXB|oQ[15]~46_combout\);

-- Location: LCCOMB_X35_Y17_N20
\iALU|Add0~78\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~78_combout\ = (\iCU|WideOr4~combout\ & (((\iCU|WideOr6~combout\)))) # (!\iCU|WideOr4~combout\ & (\iCU|WideOr5~combout\ $ (((\MUXB|oQ[15]~46_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011000111100100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr5~combout\,
	datac => \iCU|WideOr6~combout\,
	datad => \MUXB|oQ[15]~46_combout\,
	combout => \iALU|Add0~78_combout\);

-- Location: IOIBUF_X50_Y16_N15
\iDATA[14]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(14),
	o => \iDATA[14]~input_o\);

-- Location: LCCOMB_X34_Y16_N6
\iALU|Mux12~1\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Mux12~1_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((!\iCU|WideOr5~combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100000011110011",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr6~combout\,
	datad => \iCU|WideOr5~combout\,
	combout => \iALU|Mux12~1_combout\);

-- Location: FF_X33_Y16_N7
\R3|sREG[14]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~77_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(14));

-- Location: FF_X32_Y17_N7
\R2|sREG[14]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~77_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(14));

-- Location: FF_X32_Y17_N5
\R0|sREG[14]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~77_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(14));

-- Location: LCCOMB_X32_Y17_N4
\MUXB|oQ[14]~43\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[14]~43_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(14))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(14)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(14),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(14),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[14]~43_combout\);

-- Location: LCCOMB_X34_Y16_N16
\MUXB|oQ[14]~44\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[14]~44_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[14]~43_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[14]~43_combout\ & ((\R3|sREG\(14)))) # (!\MUXB|oQ[14]~43_combout\ & (\R1|sREG\(14)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(14),
	datab => \R3|sREG\(14),
	datac => \MUXB|oQ[1]~13_combout\,
	datad => \MUXB|oQ[14]~43_combout\,
	combout => \MUXB|oQ[14]~44_combout\);

-- Location: LCCOMB_X34_Y16_N24
\iALU|Add0~76\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~76_combout\ = (\iALU|Mux12~1_combout\ & (\iCU|WideOr4~combout\ $ ((\MUXA|oQ[14]~46_combout\)))) # (!\iALU|Mux12~1_combout\ & ((\iCU|WideOr4~combout\ & ((\MUXA|oQ[14]~46_combout\) # (\MUXB|oQ[14]~44_combout\))) # (!\iCU|WideOr4~combout\ & 
-- (\MUXA|oQ[14]~46_combout\ & \MUXB|oQ[14]~44_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0111110001101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~1_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXA|oQ[14]~46_combout\,
	datad => \MUXB|oQ[14]~44_combout\,
	combout => \iALU|Add0~76_combout\);

-- Location: LCCOMB_X34_Y16_N18
\iALU|Add0~73\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~73_combout\ = (\iCU|WideOr4~combout\ & (((\iCU|WideOr6~combout\)))) # (!\iCU|WideOr4~combout\ & (\iCU|WideOr5~combout\ $ (((\MUXB|oQ[14]~44_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011000111100100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr5~combout\,
	datac => \iCU|WideOr6~combout\,
	datad => \MUXB|oQ[14]~44_combout\,
	combout => \iALU|Add0~73_combout\);

-- Location: IOIBUF_X25_Y23_N22
\iDATA[13]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(13),
	o => \iDATA[13]~input_o\);

-- Location: LCCOMB_X31_Y16_N18
\R2|sREG[13]~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \R2|sREG[13]~feeder_combout\ = \iALU|Add0~72_combout\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iALU|Add0~72_combout\,
	combout => \R2|sREG[13]~feeder_combout\);

-- Location: FF_X31_Y16_N19
\R2|sREG[13]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \R2|sREG[13]~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(13));

-- Location: FF_X33_Y16_N29
\R3|sREG[13]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~72_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(13));

-- Location: LCCOMB_X33_Y18_N0
\R0|sREG[13]~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \R0|sREG[13]~feeder_combout\ = \iALU|Add0~72_combout\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iALU|Add0~72_combout\,
	combout => \R0|sREG[13]~feeder_combout\);

-- Location: FF_X33_Y18_N1
\R0|sREG[13]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \R0|sREG[13]~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(13));

-- Location: LCCOMB_X33_Y18_N6
\MUXB|oQ[13]~41\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[13]~41_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(13) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(13)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000111111001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(13),
	datab => \R0|sREG\(13),
	datac => \MUXB|oQ[1]~13_combout\,
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[13]~41_combout\);

-- Location: LCCOMB_X33_Y18_N4
\MUXB|oQ[13]~42\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[13]~42_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[13]~41_combout\ & ((\R3|sREG\(13)))) # (!\MUXB|oQ[13]~41_combout\ & (\R2|sREG\(13))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[13]~41_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111010110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~14_combout\,
	datab => \R2|sREG\(13),
	datac => \R3|sREG\(13),
	datad => \MUXB|oQ[13]~41_combout\,
	combout => \MUXB|oQ[13]~42_combout\);

-- Location: LCCOMB_X31_Y16_N14
\iALU|Add0~71\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~71_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[13]~43_combout\ & ((!\iALU|Mux12~1_combout\))) # (!\MUXA|oQ[13]~43_combout\ & ((\MUXB|oQ[13]~42_combout\) # (\iALU|Mux12~1_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[13]~43_combout\ & 
-- ((\MUXB|oQ[13]~42_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0101101011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \MUXB|oQ[13]~42_combout\,
	datac => \MUXA|oQ[13]~43_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~71_combout\);

-- Location: LCCOMB_X33_Y18_N18
\iALU|Add0~68\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~68_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\MUXB|oQ[13]~42_combout\ $ (\iCU|WideOr5~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000101110111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr6~combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXB|oQ[13]~42_combout\,
	datad => \iCU|WideOr5~combout\,
	combout => \iALU|Add0~68_combout\);

-- Location: IOIBUF_X25_Y25_N15
\iDATA[12]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(12),
	o => \iDATA[12]~input_o\);

-- Location: FF_X32_Y18_N17
\R1|sREG[12]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~67_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(12));

-- Location: FF_X33_Y18_N17
\R0|sREG[12]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~67_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(12));

-- Location: FF_X33_Y18_N31
\R2|sREG[12]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~67_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(12));

-- Location: LCCOMB_X33_Y18_N30
\MUXA|oQ[12]~38\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[12]~38_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(12)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(12)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(12),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(12),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[12]~38_combout\);

-- Location: LCCOMB_X33_Y18_N10
\MUXA|oQ[12]~39\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[12]~39_combout\ = (\MUXA|oQ[12]~38_combout\ & ((\R3|sREG\(12)) # ((!\iCU|WideOr3~combout\)))) # (!\MUXA|oQ[12]~38_combout\ & (((\R1|sREG\(12) & \iCU|WideOr3~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010110011110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(12),
	datab => \R1|sREG\(12),
	datac => \MUXA|oQ[12]~38_combout\,
	datad => \iCU|WideOr3~combout\,
	combout => \MUXA|oQ[12]~39_combout\);

-- Location: LCCOMB_X33_Y18_N28
\MUXA|oQ[12]~40\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[12]~40_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[12]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[12]~39_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iDATA[12]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[12]~39_combout\,
	combout => \MUXA|oQ[12]~40_combout\);

-- Location: LCCOMB_X33_Y18_N26
\iALU|Add0~66\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~66_combout\ = (\MUXA|oQ[12]~40_combout\ & ((\iCU|WideOr4~combout\ & ((!\iALU|Mux12~1_combout\))) # (!\iCU|WideOr4~combout\ & ((\MUXB|oQ[12]~40_combout\) # (\iALU|Mux12~1_combout\))))) # (!\MUXA|oQ[12]~40_combout\ & (\iCU|WideOr4~combout\ & 
-- ((\MUXB|oQ[12]~40_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011110011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[12]~40_combout\,
	datab => \MUXA|oQ[12]~40_combout\,
	datac => \iCU|WideOr4~combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~66_combout\);

-- Location: IOIBUF_X25_Y22_N15
\iDATA[11]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(11),
	o => \iDATA[11]~input_o\);

-- Location: FF_X33_Y18_N23
\R2|sREG[11]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~62_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(11));

-- Location: FF_X32_Y18_N25
\R1|sREG[11]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~62_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(11));

-- Location: FF_X33_Y18_N13
\R0|sREG[11]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~62_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(11));

-- Location: LCCOMB_X33_Y18_N12
\MUXB|oQ[11]~37\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[11]~37_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(11) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(11)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011001111100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(11),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(11),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[11]~37_combout\);

-- Location: LCCOMB_X32_Y18_N10
\MUXB|oQ[11]~38\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[11]~38_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[11]~37_combout\ & ((\R3|sREG\(11)))) # (!\MUXB|oQ[11]~37_combout\ & (\R2|sREG\(11))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[11]~37_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(11),
	datab => \MUXB|oQ[1]~14_combout\,
	datac => \R3|sREG\(11),
	datad => \MUXB|oQ[11]~37_combout\,
	combout => \MUXB|oQ[11]~38_combout\);

-- Location: LCCOMB_X34_Y16_N2
\iALU|Add0~61\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~61_combout\ = (\iALU|Mux12~1_combout\ & (\iCU|WideOr4~combout\ $ (((\MUXA|oQ[11]~37_combout\))))) # (!\iALU|Mux12~1_combout\ & ((\iCU|WideOr4~combout\ & ((\MUXB|oQ[11]~38_combout\) # (\MUXA|oQ[11]~37_combout\))) # (!\iCU|WideOr4~combout\ & 
-- (\MUXB|oQ[11]~38_combout\ & \MUXA|oQ[11]~37_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0111011011001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~1_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXB|oQ[11]~38_combout\,
	datad => \MUXA|oQ[11]~37_combout\,
	combout => \iALU|Add0~61_combout\);

-- Location: LCCOMB_X32_Y18_N8
\iALU|Add0~58\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~58_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[11]~38_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000101110111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr6~combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[11]~38_combout\,
	combout => \iALU|Add0~58_combout\);

-- Location: IOIBUF_X25_Y25_N22
\iDATA[10]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(10),
	o => \iDATA[10]~input_o\);

-- Location: FF_X32_Y18_N7
\R1|sREG[10]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~57_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(10));

-- Location: FF_X32_Y18_N13
\R2|sREG[10]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~57_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(10));

-- Location: FF_X32_Y17_N13
\R0|sREG[10]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~57_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(10));

-- Location: LCCOMB_X32_Y17_N12
\MUXB|oQ[10]~35\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[10]~35_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(10))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(10)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R2|sREG\(10),
	datac => \R0|sREG\(10),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[10]~35_combout\);

-- Location: LCCOMB_X32_Y18_N6
\MUXB|oQ[10]~36\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[10]~36_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[10]~35_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[10]~35_combout\ & (\R3|sREG\(10))) # (!\MUXB|oQ[10]~35_combout\ & ((\R1|sREG\(10))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(10),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R1|sREG\(10),
	datad => \MUXB|oQ[10]~35_combout\,
	combout => \MUXB|oQ[10]~36_combout\);

-- Location: LCCOMB_X34_Y16_N20
\iALU|Add0~56\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~56_combout\ = (\iALU|Mux12~1_combout\ & ((\MUXA|oQ[10]~34_combout\ $ (\iCU|WideOr4~combout\)))) # (!\iALU|Mux12~1_combout\ & ((\MUXB|oQ[10]~36_combout\ & ((\MUXA|oQ[10]~34_combout\) # (\iCU|WideOr4~combout\))) # (!\MUXB|oQ[10]~36_combout\ & 
-- (\MUXA|oQ[10]~34_combout\ & \iCU|WideOr4~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0101111011100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~1_combout\,
	datab => \MUXB|oQ[10]~36_combout\,
	datac => \MUXA|oQ[10]~34_combout\,
	datad => \iCU|WideOr4~combout\,
	combout => \iALU|Add0~56_combout\);

-- Location: LCCOMB_X32_Y18_N22
\iALU|Add0~53\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~53_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[10]~36_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000101110111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr6~combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[10]~36_combout\,
	combout => \iALU|Add0~53_combout\);

-- Location: IOIBUF_X50_Y22_N15
\iDATA[9]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(9),
	o => \iDATA[9]~input_o\);

-- Location: FF_X33_Y16_N19
\R3|sREG[9]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~52_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(9));

-- Location: FF_X32_Y18_N1
\R1|sREG[9]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~52_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(9));

-- Location: FF_X33_Y18_N25
\R0|sREG[9]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~52_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(9));

-- Location: LCCOMB_X33_Y18_N2
\MUXA|oQ[9]~29\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[9]~29_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(9)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(9)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(9),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(9),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[9]~29_combout\);

-- Location: LCCOMB_X32_Y18_N0
\MUXA|oQ[9]~30\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[9]~30_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[9]~29_combout\ & (\R3|sREG\(9))) # (!\MUXA|oQ[9]~29_combout\ & ((\R1|sREG\(9)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[9]~29_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(9),
	datab => \iCU|WideOr3~combout\,
	datac => \R1|sREG\(9),
	datad => \MUXA|oQ[9]~29_combout\,
	combout => \MUXA|oQ[9]~30_combout\);

-- Location: LCCOMB_X32_Y18_N26
\MUXA|oQ[9]~31\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[9]~31_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[9]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[9]~30_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iDATA[9]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[9]~30_combout\,
	combout => \MUXA|oQ[9]~31_combout\);

-- Location: IOIBUF_X16_Y17_N22
\iDATA[8]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(8),
	o => \iDATA[8]~input_o\);

-- Location: FF_X32_Y16_N15
\R1|sREG[8]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~47_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(8));

-- Location: FF_X31_Y16_N13
\R2|sREG[8]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~47_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(8));

-- Location: FF_X32_Y17_N15
\R0|sREG[8]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~47_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(8));

-- Location: LCCOMB_X32_Y17_N14
\MUXB|oQ[8]~31\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[8]~31_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(8))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(8)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R2|sREG\(8),
	datac => \R0|sREG\(8),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[8]~31_combout\);

-- Location: LCCOMB_X32_Y16_N14
\MUXB|oQ[8]~32\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[8]~32_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[8]~31_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[8]~31_combout\ & (\R3|sREG\(8))) # (!\MUXB|oQ[8]~31_combout\ & ((\R1|sREG\(8))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(8),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R1|sREG\(8),
	datad => \MUXB|oQ[8]~31_combout\,
	combout => \MUXB|oQ[8]~32_combout\);

-- Location: LCCOMB_X31_Y16_N4
\iALU|Add0~46\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~46_combout\ = (\MUXA|oQ[8]~28_combout\ & ((\iCU|WideOr4~combout\ & ((!\iALU|Mux12~1_combout\))) # (!\iCU|WideOr4~combout\ & ((\MUXB|oQ[8]~32_combout\) # (\iALU|Mux12~1_combout\))))) # (!\MUXA|oQ[8]~28_combout\ & (\iCU|WideOr4~combout\ & 
-- ((\MUXB|oQ[8]~32_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110011011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[8]~28_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXB|oQ[8]~32_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~46_combout\);

-- Location: LCCOMB_X32_Y16_N30
\iALU|Add0~43\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~43_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\MUXB|oQ[8]~32_combout\ $ (\iCU|WideOr5~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000110111011000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr6~combout\,
	datac => \MUXB|oQ[8]~32_combout\,
	datad => \iCU|WideOr5~combout\,
	combout => \iALU|Add0~43_combout\);

-- Location: IOIBUF_X25_Y24_N22
\iDATA[7]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(7),
	o => \iDATA[7]~input_o\);

-- Location: FF_X32_Y16_N3
\R1|sREG[7]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~42_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(7));

-- Location: FF_X33_Y18_N15
\R0|sREG[7]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~42_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(7));

-- Location: FF_X33_Y18_N21
\R2|sREG[7]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~42_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(7));

-- Location: LCCOMB_X33_Y18_N20
\MUXA|oQ[7]~23\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[7]~23_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(7)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(7)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(7),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(7),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[7]~23_combout\);

-- Location: LCCOMB_X32_Y16_N2
\MUXA|oQ[7]~24\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[7]~24_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[7]~23_combout\ & (\R3|sREG\(7))) # (!\MUXA|oQ[7]~23_combout\ & ((\R1|sREG\(7)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[7]~23_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr3~combout\,
	datab => \R3|sREG\(7),
	datac => \R1|sREG\(7),
	datad => \MUXA|oQ[7]~23_combout\,
	combout => \MUXA|oQ[7]~24_combout\);

-- Location: LCCOMB_X32_Y16_N6
\MUXA|oQ[7]~25\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[7]~25_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[7]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[7]~24_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T1~q\,
	datac => \iDATA[7]~input_o\,
	datad => \MUXA|oQ[7]~24_combout\,
	combout => \MUXA|oQ[7]~25_combout\);

-- Location: IOIBUF_X50_Y15_N15
\iDATA[6]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(6),
	o => \iDATA[6]~input_o\);

-- Location: FF_X32_Y16_N21
\R1|sREG[6]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~37_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(6));

-- Location: FF_X32_Y17_N21
\R0|sREG[6]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~37_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(6));

-- Location: FF_X32_Y17_N19
\R2|sREG[6]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~37_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(6));

-- Location: LCCOMB_X32_Y17_N18
\MUXA|oQ[6]~20\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[6]~20_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(6)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(6)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(6),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(6),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[6]~20_combout\);

-- Location: LCCOMB_X32_Y16_N18
\MUXA|oQ[6]~21\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[6]~21_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[6]~20_combout\ & ((\R3|sREG\(6)))) # (!\MUXA|oQ[6]~20_combout\ & (\R1|sREG\(6))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[6]~20_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(6),
	datab => \R3|sREG\(6),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[6]~20_combout\,
	combout => \MUXA|oQ[6]~21_combout\);

-- Location: LCCOMB_X32_Y16_N0
\MUXA|oQ[6]~22\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[6]~22_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[6]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[6]~21_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iDATA[6]~input_o\,
	datab => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[6]~21_combout\,
	combout => \MUXA|oQ[6]~22_combout\);

-- Location: LCCOMB_X32_Y16_N28
\iALU|Add0~36\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~36_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[6]~22_combout\ & (!\iALU|Mux12~1_combout\)) # (!\MUXA|oQ[6]~22_combout\ & ((\iALU|Mux12~1_combout\) # (\MUXB|oQ[6]~28_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[6]~22_combout\ & 
-- ((\iALU|Mux12~1_combout\) # (\MUXB|oQ[6]~28_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110111001101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \MUXA|oQ[6]~22_combout\,
	datac => \iALU|Mux12~1_combout\,
	datad => \MUXB|oQ[6]~28_combout\,
	combout => \iALU|Add0~36_combout\);

-- Location: IOIBUF_X50_Y21_N23
\iDATA[5]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(5),
	o => \iDATA[5]~input_o\);

-- Location: FF_X32_Y16_N23
\R3|sREG[5]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~32_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(5));

-- Location: FF_X32_Y16_N1
\R1|sREG[5]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~32_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(5));

-- Location: FF_X32_Y17_N27
\R0|sREG[5]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~32_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(5));

-- Location: LCCOMB_X35_Y17_N22
\MUXA|oQ[5]~17\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[5]~17_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(5)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(5)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(5),
	datab => \R2|sREG\(5),
	datac => \iCU|WideOr3~combout\,
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[5]~17_combout\);

-- Location: LCCOMB_X34_Y17_N18
\MUXA|oQ[5]~18\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[5]~18_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[5]~17_combout\ & (\R3|sREG\(5))) # (!\MUXA|oQ[5]~17_combout\ & ((\R1|sREG\(5)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[5]~17_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr3~combout\,
	datab => \R3|sREG\(5),
	datac => \R1|sREG\(5),
	datad => \MUXA|oQ[5]~17_combout\,
	combout => \MUXA|oQ[5]~18_combout\);

-- Location: LCCOMB_X34_Y17_N8
\MUXA|oQ[5]~19\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[5]~19_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[5]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[5]~18_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T1~q\,
	datac => \iDATA[5]~input_o\,
	datad => \MUXA|oQ[5]~18_combout\,
	combout => \MUXA|oQ[5]~19_combout\);

-- Location: IOIBUF_X24_Y0_N29
\iDATA[4]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(4),
	o => \iDATA[4]~input_o\);

-- Location: FF_X35_Y17_N5
\R1|sREG[4]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~27_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(4));

-- Location: FF_X32_Y17_N17
\R0|sREG[4]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~27_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(4));

-- Location: LCCOMB_X35_Y17_N14
\R2|sREG[4]~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \R2|sREG[4]~feeder_combout\ = \iALU|Add0~27_combout\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iALU|Add0~27_combout\,
	combout => \R2|sREG[4]~feeder_combout\);

-- Location: FF_X35_Y17_N15
\R2|sREG[4]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \R2|sREG[4]~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(4));

-- Location: LCCOMB_X35_Y17_N12
\MUXA|oQ[4]~14\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[4]~14_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(4)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(4)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(4),
	datab => \R2|sREG\(4),
	datac => \iCU|WideOr3~combout\,
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[4]~14_combout\);

-- Location: LCCOMB_X35_Y17_N2
\MUXA|oQ[4]~15\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[4]~15_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[4]~14_combout\ & ((\R3|sREG\(4)))) # (!\MUXA|oQ[4]~14_combout\ & (\R1|sREG\(4))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[4]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(4),
	datab => \R3|sREG\(4),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[4]~14_combout\,
	combout => \MUXA|oQ[4]~15_combout\);

-- Location: LCCOMB_X35_Y17_N24
\MUXA|oQ[4]~16\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[4]~16_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[4]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[4]~15_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iDATA[4]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[4]~15_combout\,
	combout => \MUXA|oQ[4]~16_combout\);

-- Location: LCCOMB_X34_Y17_N4
\iALU|Add0~26\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~26_combout\ = (\MUXA|oQ[4]~16_combout\ & ((\iCU|WideOr4~combout\ & ((!\iALU|Mux12~1_combout\))) # (!\iCU|WideOr4~combout\ & ((\MUXB|oQ[4]~24_combout\) # (\iALU|Mux12~1_combout\))))) # (!\MUXA|oQ[4]~16_combout\ & (\iCU|WideOr4~combout\ & 
-- ((\MUXB|oQ[4]~24_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011110011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[4]~24_combout\,
	datab => \MUXA|oQ[4]~16_combout\,
	datac => \iCU|WideOr4~combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~26_combout\);

-- Location: IOIBUF_X12_Y17_N22
\iDATA[3]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(3),
	o => \iDATA[3]~input_o\);

-- Location: FF_X34_Y17_N25
\R3|sREG[3]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~22_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(3));

-- Location: LCCOMB_X35_Y17_N26
\R1|sREG[3]~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \R1|sREG[3]~feeder_combout\ = \iALU|Add0~22_combout\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iALU|Add0~22_combout\,
	combout => \R1|sREG[3]~feeder_combout\);

-- Location: FF_X35_Y17_N27
\R1|sREG[3]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \R1|sREG[3]~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(3));

-- Location: FF_X32_Y17_N25
\R0|sREG[3]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~22_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(3));

-- Location: LCCOMB_X35_Y17_N28
\MUXA|oQ[3]~11\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[3]~11_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(3)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(3)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(3),
	datab => \R2|sREG\(3),
	datac => \iCU|WideOr3~combout\,
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[3]~11_combout\);

-- Location: LCCOMB_X35_Y17_N18
\MUXA|oQ[3]~12\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[3]~12_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[3]~11_combout\ & (\R3|sREG\(3))) # (!\MUXA|oQ[3]~11_combout\ & ((\R1|sREG\(3)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[3]~11_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010111111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(3),
	datab => \R1|sREG\(3),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[3]~11_combout\,
	combout => \MUXA|oQ[3]~12_combout\);

-- Location: LCCOMB_X35_Y17_N0
\MUXA|oQ[3]~13\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[3]~13_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[3]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[3]~12_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T1~q\,
	datac => \iDATA[3]~input_o\,
	datad => \MUXA|oQ[3]~12_combout\,
	combout => \MUXA|oQ[3]~13_combout\);

-- Location: LCCOMB_X35_Y17_N30
\iALU|Add0~21\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~21_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[3]~13_combout\ & ((!\iALU|Mux12~1_combout\))) # (!\MUXA|oQ[3]~13_combout\ & ((\MUXB|oQ[3]~22_combout\) # (\iALU|Mux12~1_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[3]~13_combout\ & 
-- ((\MUXB|oQ[3]~22_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110011011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \MUXA|oQ[3]~13_combout\,
	datac => \MUXB|oQ[3]~22_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~21_combout\);

-- Location: IOIBUF_X14_Y17_N22
\iDATA[2]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(2),
	o => \iDATA[2]~input_o\);

-- Location: FF_X34_Y17_N13
\R3|sREG[2]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~17_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(2));

-- Location: FF_X31_Y17_N9
\R2|sREG[2]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~17_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(2));

-- Location: FF_X32_Y17_N23
\R0|sREG[2]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~17_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(2));

-- Location: LCCOMB_X32_Y17_N22
\MUXB|oQ[2]~19\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[2]~19_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(2))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(2)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(2),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(2),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[2]~19_combout\);

-- Location: LCCOMB_X31_Y17_N6
\MUXB|oQ[2]~20\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[2]~20_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[2]~19_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[2]~19_combout\ & (\R3|sREG\(2))) # (!\MUXB|oQ[2]~19_combout\ & ((\R1|sREG\(2))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(2),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R1|sREG\(2),
	datad => \MUXB|oQ[2]~19_combout\,
	combout => \MUXB|oQ[2]~20_combout\);

-- Location: LCCOMB_X31_Y17_N16
\iALU|Add0~16\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~16_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[2]~10_combout\ & ((!\iALU|Mux12~1_combout\))) # (!\MUXA|oQ[2]~10_combout\ & ((\MUXB|oQ[2]~20_combout\) # (\iALU|Mux12~1_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[2]~10_combout\ & 
-- ((\MUXB|oQ[2]~20_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011110011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[2]~20_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXA|oQ[2]~10_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~16_combout\);

-- Location: LCCOMB_X31_Y17_N28
\iALU|Add0~13\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~13_combout\ = (\iCU|WideOr4~combout\ & (((\iCU|WideOr6~combout\)))) # (!\iCU|WideOr4~combout\ & (\iCU|WideOr5~combout\ $ (((\MUXB|oQ[2]~20_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011000111100100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr5~combout\,
	datac => \iCU|WideOr6~combout\,
	datad => \MUXB|oQ[2]~20_combout\,
	combout => \iALU|Add0~13_combout\);

-- Location: IOIBUF_X19_Y17_N8
\iDATA[1]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(1),
	o => \iDATA[1]~input_o\);

-- Location: FF_X31_Y17_N13
\R1|sREG[1]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~12_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(1));

-- Location: FF_X32_Y17_N9
\R0|sREG[1]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~12_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE\(0),
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R0|sREG\(1));

-- Location: FF_X31_Y17_N11
\R2|sREG[1]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~12_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(1));

-- Location: LCCOMB_X31_Y17_N10
\MUXA|oQ[1]~5\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[1]~5_combout\ = (\iCU|WideOr2~combout\ & (((\R2|sREG\(1)) # (\iCU|WideOr3~combout\)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(1) & ((!\iCU|WideOr3~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100110011100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(1),
	datab => \iCU|WideOr2~combout\,
	datac => \R2|sREG\(1),
	datad => \iCU|WideOr3~combout\,
	combout => \MUXA|oQ[1]~5_combout\);

-- Location: LCCOMB_X31_Y17_N12
\MUXA|oQ[1]~6\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[1]~6_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[1]~5_combout\ & (\R3|sREG\(1))) # (!\MUXA|oQ[1]~5_combout\ & ((\R1|sREG\(1)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[1]~5_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr3~combout\,
	datab => \R3|sREG\(1),
	datac => \R1|sREG\(1),
	datad => \MUXA|oQ[1]~5_combout\,
	combout => \MUXA|oQ[1]~6_combout\);

-- Location: LCCOMB_X31_Y17_N14
\MUXA|oQ[1]~7\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[1]~7_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[1]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[1]~6_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T1~q\,
	datac => \iDATA[1]~input_o\,
	datad => \MUXA|oQ[1]~6_combout\,
	combout => \MUXA|oQ[1]~7_combout\);

-- Location: LCCOMB_X31_Y17_N20
\iALU|Add0~11\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~11_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[1]~7_combout\ & ((!\iALU|Mux12~1_combout\))) # (!\MUXA|oQ[1]~7_combout\ & ((\MUXB|oQ[1]~18_combout\) # (\iALU|Mux12~1_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[1]~7_combout\ & 
-- ((\MUXB|oQ[1]~18_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0101101011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \MUXB|oQ[1]~18_combout\,
	datac => \MUXA|oQ[1]~7_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~11_combout\);

-- Location: IOIBUF_X16_Y17_N1
\iDATA[0]~input\ : fiftyfivenm_io_ibuf
-- pragma translate_off
GENERIC MAP (
	bus_hold => "false",
	listen_to_nsleep_signal => "false",
	simulate_z_as => "z")
-- pragma translate_on
PORT MAP (
	i => ww_iDATA(0),
	o => \iDATA[0]~input_o\);

-- Location: LCCOMB_X33_Y17_N8
\MUXA|oQ[0]~1\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[0]~1_combout\ = (!\iCU|currentState.T3~q\ & (!\iCU|currentState.T5~q\ & (!\iCU|currentState.T9~q\ & !\iCU|oREG_WE[1]~0_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000000001",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T3~q\,
	datab => \iCU|currentState.T5~q\,
	datac => \iCU|currentState.T9~q\,
	datad => \iCU|oREG_WE[1]~0_combout\,
	combout => \MUXA|oQ[0]~1_combout\);

-- Location: LCCOMB_X33_Y17_N12
\MUXA|oQ[0]~2\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[0]~2_combout\ = (\MUXA|oQ[0]~1_combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(0)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(0))))) # (!\MUXA|oQ[0]~1_combout\ & (((\iCU|WideOr2~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(0),
	datab => \MUXA|oQ[0]~1_combout\,
	datac => \R2|sREG\(0),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[0]~2_combout\);

-- Location: LCCOMB_X33_Y17_N10
\MUXA|oQ[0]~3\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[0]~3_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[0]~2_combout\ & (\R3|sREG\(0))) # (!\MUXA|oQ[0]~2_combout\ & ((\R1|sREG\(0)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[0]~2_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(0),
	datab => \iCU|WideOr3~combout\,
	datac => \R1|sREG\(0),
	datad => \MUXA|oQ[0]~2_combout\,
	combout => \MUXA|oQ[0]~3_combout\);

-- Location: LCCOMB_X33_Y17_N2
\MUXA|oQ[0]~4\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[0]~4_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[0]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[0]~3_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T1~q\,
	datab => \iDATA[0]~input_o\,
	datad => \MUXA|oQ[0]~3_combout\,
	combout => \MUXA|oQ[0]~4_combout\);

-- Location: LCCOMB_X33_Y17_N16
\iALU|Add0~3\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~3_cout\ = CARRY((!\iCU|WideOr4~combout\ & !\iCU|WideOr6~combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000010001",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr6~combout\,
	datad => VCC,
	cout => \iALU|Add0~3_cout\);

-- Location: LCCOMB_X33_Y17_N18
\iALU|Add0~4\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~4_combout\ = (\iALU|Add0~1_combout\ & ((\MUXA|oQ[0]~4_combout\ & (\iALU|Add0~3_cout\ & VCC)) # (!\MUXA|oQ[0]~4_combout\ & (!\iALU|Add0~3_cout\)))) # (!\iALU|Add0~1_combout\ & ((\MUXA|oQ[0]~4_combout\ & (!\iALU|Add0~3_cout\)) # 
-- (!\MUXA|oQ[0]~4_combout\ & ((\iALU|Add0~3_cout\) # (GND)))))
-- \iALU|Add0~5\ = CARRY((\iALU|Add0~1_combout\ & (!\MUXA|oQ[0]~4_combout\ & !\iALU|Add0~3_cout\)) # (!\iALU|Add0~1_combout\ & ((!\iALU|Add0~3_cout\) # (!\MUXA|oQ[0]~4_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~1_combout\,
	datab => \MUXA|oQ[0]~4_combout\,
	datad => VCC,
	cin => \iALU|Add0~3_cout\,
	combout => \iALU|Add0~4_combout\,
	cout => \iALU|Add0~5\);

-- Location: LCCOMB_X33_Y17_N20
\iALU|Add0~9\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~9_combout\ = ((\iALU|Add0~8_combout\ $ (\MUXA|oQ[1]~7_combout\ $ (!\iALU|Add0~5\)))) # (GND)
-- \iALU|Add0~10\ = CARRY((\iALU|Add0~8_combout\ & ((\MUXA|oQ[1]~7_combout\) # (!\iALU|Add0~5\))) # (!\iALU|Add0~8_combout\ & (\MUXA|oQ[1]~7_combout\ & !\iALU|Add0~5\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~8_combout\,
	datab => \MUXA|oQ[1]~7_combout\,
	datad => VCC,
	cin => \iALU|Add0~5\,
	combout => \iALU|Add0~9_combout\,
	cout => \iALU|Add0~10\);

-- Location: LCCOMB_X34_Y17_N26
\iALU|Add0~12\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~12_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~9_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~11_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~11_combout\,
	datad => \iALU|Add0~9_combout\,
	combout => \iALU|Add0~12_combout\);

-- Location: FF_X34_Y17_N27
\R3|sREG[1]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~12_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(1));

-- Location: LCCOMB_X32_Y17_N8
\MUXB|oQ[1]~17\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[1]~17_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(1) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(1)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011001111100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(1),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(1),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[1]~17_combout\);

-- Location: LCCOMB_X31_Y17_N2
\MUXB|oQ[1]~18\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[1]~18_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[1]~17_combout\ & (\R3|sREG\(1))) # (!\MUXB|oQ[1]~17_combout\ & ((\R2|sREG\(1)))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[1]~17_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101101011010000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~14_combout\,
	datab => \R3|sREG\(1),
	datac => \MUXB|oQ[1]~17_combout\,
	datad => \R2|sREG\(1),
	combout => \MUXB|oQ[1]~18_combout\);

-- Location: LCCOMB_X31_Y17_N4
\iALU|Add0~8\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~8_combout\ = (\iCU|WideOr4~combout\ & (((\iCU|WideOr6~combout\)))) # (!\iCU|WideOr4~combout\ & (\iCU|WideOr5~combout\ $ (((\MUXB|oQ[1]~18_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011000111100100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr5~combout\,
	datac => \iCU|WideOr6~combout\,
	datad => \MUXB|oQ[1]~18_combout\,
	combout => \iALU|Add0~8_combout\);

-- Location: LCCOMB_X33_Y17_N22
\iALU|Add0~14\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~14_combout\ = (\MUXA|oQ[2]~10_combout\ & ((\iALU|Add0~13_combout\ & (\iALU|Add0~10\ & VCC)) # (!\iALU|Add0~13_combout\ & (!\iALU|Add0~10\)))) # (!\MUXA|oQ[2]~10_combout\ & ((\iALU|Add0~13_combout\ & (!\iALU|Add0~10\)) # (!\iALU|Add0~13_combout\ 
-- & ((\iALU|Add0~10\) # (GND)))))
-- \iALU|Add0~15\ = CARRY((\MUXA|oQ[2]~10_combout\ & (!\iALU|Add0~13_combout\ & !\iALU|Add0~10\)) # (!\MUXA|oQ[2]~10_combout\ & ((!\iALU|Add0~10\) # (!\iALU|Add0~13_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[2]~10_combout\,
	datab => \iALU|Add0~13_combout\,
	datad => VCC,
	cin => \iALU|Add0~10\,
	combout => \iALU|Add0~14_combout\,
	cout => \iALU|Add0~15\);

-- Location: LCCOMB_X34_Y17_N30
\iALU|Add0~17\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~17_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~14_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~16_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~16_combout\,
	datad => \iALU|Add0~14_combout\,
	combout => \iALU|Add0~17_combout\);

-- Location: FF_X31_Y17_N7
\R1|sREG[2]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~17_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(2));

-- Location: LCCOMB_X31_Y17_N18
\MUXA|oQ[2]~8\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[2]~8_combout\ = (\iCU|WideOr2~combout\ & (((\R2|sREG\(2)) # (\iCU|WideOr3~combout\)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(2) & ((!\iCU|WideOr3~combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100110011100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(2),
	datab => \iCU|WideOr2~combout\,
	datac => \R2|sREG\(2),
	datad => \iCU|WideOr3~combout\,
	combout => \MUXA|oQ[2]~8_combout\);

-- Location: LCCOMB_X31_Y17_N24
\MUXA|oQ[2]~9\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[2]~9_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[2]~8_combout\ & ((\R3|sREG\(2)))) # (!\MUXA|oQ[2]~8_combout\ & (\R1|sREG\(2))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[2]~8_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(2),
	datab => \R3|sREG\(2),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[2]~8_combout\,
	combout => \MUXA|oQ[2]~9_combout\);

-- Location: LCCOMB_X31_Y17_N22
\MUXA|oQ[2]~10\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[2]~10_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[2]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[2]~9_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T1~q\,
	datac => \iDATA[2]~input_o\,
	datad => \MUXA|oQ[2]~9_combout\,
	combout => \MUXA|oQ[2]~10_combout\);

-- Location: LCCOMB_X33_Y17_N24
\iALU|Add0~19\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~19_combout\ = ((\iALU|Add0~18_combout\ $ (\MUXA|oQ[3]~13_combout\ $ (!\iALU|Add0~15\)))) # (GND)
-- \iALU|Add0~20\ = CARRY((\iALU|Add0~18_combout\ & ((\MUXA|oQ[3]~13_combout\) # (!\iALU|Add0~15\))) # (!\iALU|Add0~18_combout\ & (\MUXA|oQ[3]~13_combout\ & !\iALU|Add0~15\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~18_combout\,
	datab => \MUXA|oQ[3]~13_combout\,
	datad => VCC,
	cin => \iALU|Add0~15\,
	combout => \iALU|Add0~19_combout\,
	cout => \iALU|Add0~20\);

-- Location: LCCOMB_X34_Y17_N22
\iALU|Add0~22\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~22_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~19_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~21_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~21_combout\,
	datab => \iALU|Mux12~0_combout\,
	datad => \iALU|Add0~19_combout\,
	combout => \iALU|Add0~22_combout\);

-- Location: LCCOMB_X35_Y17_N16
\R2|sREG[3]~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \R2|sREG[3]~feeder_combout\ = \iALU|Add0~22_combout\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iALU|Add0~22_combout\,
	combout => \R2|sREG[3]~feeder_combout\);

-- Location: FF_X35_Y17_N17
\R2|sREG[3]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \R2|sREG[3]~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(3));

-- Location: LCCOMB_X32_Y17_N24
\MUXB|oQ[3]~21\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[3]~21_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(3) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(3)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011001111100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(3),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(3),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[3]~21_combout\);

-- Location: LCCOMB_X32_Y17_N2
\MUXB|oQ[3]~22\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[3]~22_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[3]~21_combout\ & ((\R3|sREG\(3)))) # (!\MUXB|oQ[3]~21_combout\ & (\R2|sREG\(3))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[3]~21_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(3),
	datab => \MUXB|oQ[1]~14_combout\,
	datac => \R3|sREG\(3),
	datad => \MUXB|oQ[3]~21_combout\,
	combout => \MUXB|oQ[3]~22_combout\);

-- Location: LCCOMB_X31_Y17_N26
\iALU|Add0~18\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~18_combout\ = (\iCU|WideOr4~combout\ & (((\iCU|WideOr6~combout\)))) # (!\iCU|WideOr4~combout\ & (\iCU|WideOr5~combout\ $ (((\MUXB|oQ[3]~22_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011000111100100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr5~combout\,
	datac => \iCU|WideOr6~combout\,
	datad => \MUXB|oQ[3]~22_combout\,
	combout => \iALU|Add0~18_combout\);

-- Location: LCCOMB_X33_Y17_N26
\iALU|Add0~24\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~24_combout\ = (\iALU|Add0~23_combout\ & ((\MUXA|oQ[4]~16_combout\ & (\iALU|Add0~20\ & VCC)) # (!\MUXA|oQ[4]~16_combout\ & (!\iALU|Add0~20\)))) # (!\iALU|Add0~23_combout\ & ((\MUXA|oQ[4]~16_combout\ & (!\iALU|Add0~20\)) # 
-- (!\MUXA|oQ[4]~16_combout\ & ((\iALU|Add0~20\) # (GND)))))
-- \iALU|Add0~25\ = CARRY((\iALU|Add0~23_combout\ & (!\MUXA|oQ[4]~16_combout\ & !\iALU|Add0~20\)) # (!\iALU|Add0~23_combout\ & ((!\iALU|Add0~20\) # (!\MUXA|oQ[4]~16_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~23_combout\,
	datab => \MUXA|oQ[4]~16_combout\,
	datad => VCC,
	cin => \iALU|Add0~20\,
	combout => \iALU|Add0~24_combout\,
	cout => \iALU|Add0~25\);

-- Location: LCCOMB_X34_Y17_N28
\iALU|Add0~27\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~27_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~24_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~26_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111000011001100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iALU|Add0~26_combout\,
	datac => \iALU|Add0~24_combout\,
	datad => \iALU|Mux12~0_combout\,
	combout => \iALU|Add0~27_combout\);

-- Location: FF_X34_Y17_N29
\R3|sREG[4]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~27_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(4));

-- Location: LCCOMB_X32_Y17_N16
\MUXB|oQ[4]~23\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[4]~23_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(4))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(4)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(4),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(4),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[4]~23_combout\);

-- Location: LCCOMB_X35_Y17_N4
\MUXB|oQ[4]~24\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[4]~24_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[4]~23_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[4]~23_combout\ & (\R3|sREG\(4))) # (!\MUXB|oQ[4]~23_combout\ & ((\R1|sREG\(4))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001010000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R3|sREG\(4),
	datac => \R1|sREG\(4),
	datad => \MUXB|oQ[4]~23_combout\,
	combout => \MUXB|oQ[4]~24_combout\);

-- Location: LCCOMB_X34_Y17_N6
\iALU|Add0~23\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~23_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[4]~24_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000101110111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr6~combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[4]~24_combout\,
	combout => \iALU|Add0~23_combout\);

-- Location: LCCOMB_X33_Y17_N28
\iALU|Add0~29\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~29_combout\ = ((\iALU|Add0~28_combout\ $ (\MUXA|oQ[5]~19_combout\ $ (!\iALU|Add0~25\)))) # (GND)
-- \iALU|Add0~30\ = CARRY((\iALU|Add0~28_combout\ & ((\MUXA|oQ[5]~19_combout\) # (!\iALU|Add0~25\))) # (!\iALU|Add0~28_combout\ & (\MUXA|oQ[5]~19_combout\ & !\iALU|Add0~25\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~28_combout\,
	datab => \MUXA|oQ[5]~19_combout\,
	datad => VCC,
	cin => \iALU|Add0~25\,
	combout => \iALU|Add0~29_combout\,
	cout => \iALU|Add0~30\);

-- Location: LCCOMB_X31_Y16_N22
\iALU|Add0~31\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~31_combout\ = (\iALU|Mux12~1_combout\ & (\iCU|WideOr4~combout\ $ (((\MUXA|oQ[5]~19_combout\))))) # (!\iALU|Mux12~1_combout\ & ((\iCU|WideOr4~combout\ & ((\MUXB|oQ[5]~26_combout\) # (\MUXA|oQ[5]~19_combout\))) # (!\iCU|WideOr4~combout\ & 
-- (\MUXB|oQ[5]~26_combout\ & \MUXA|oQ[5]~19_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0111011011001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~1_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXB|oQ[5]~26_combout\,
	datad => \MUXA|oQ[5]~19_combout\,
	combout => \iALU|Add0~31_combout\);

-- Location: LCCOMB_X32_Y16_N22
\iALU|Add0~32\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~32_combout\ = (\iALU|Mux12~0_combout\ & (\iALU|Add0~29_combout\)) # (!\iALU|Mux12~0_combout\ & ((\iALU|Add0~31_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111010110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~29_combout\,
	datad => \iALU|Add0~31_combout\,
	combout => \iALU|Add0~32_combout\);

-- Location: FF_X33_Y17_N7
\R2|sREG[5]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~32_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(5));

-- Location: LCCOMB_X32_Y17_N26
\MUXB|oQ[5]~25\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[5]~25_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(5) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(5)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011001111100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(5),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(5),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[5]~25_combout\);

-- Location: LCCOMB_X32_Y17_N28
\MUXB|oQ[5]~26\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[5]~26_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[5]~25_combout\ & ((\R3|sREG\(5)))) # (!\MUXB|oQ[5]~25_combout\ & (\R2|sREG\(5))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[5]~25_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111100000111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(5),
	datab => \MUXB|oQ[1]~14_combout\,
	datac => \MUXB|oQ[5]~25_combout\,
	datad => \R3|sREG\(5),
	combout => \MUXB|oQ[5]~26_combout\);

-- Location: LCCOMB_X30_Y17_N2
\iALU|Add0~28\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~28_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[5]~26_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000110111011000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr6~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[5]~26_combout\,
	combout => \iALU|Add0~28_combout\);

-- Location: LCCOMB_X33_Y17_N30
\iALU|Add0~34\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~34_combout\ = (\iALU|Add0~33_combout\ & ((\MUXA|oQ[6]~22_combout\ & (\iALU|Add0~30\ & VCC)) # (!\MUXA|oQ[6]~22_combout\ & (!\iALU|Add0~30\)))) # (!\iALU|Add0~33_combout\ & ((\MUXA|oQ[6]~22_combout\ & (!\iALU|Add0~30\)) # 
-- (!\MUXA|oQ[6]~22_combout\ & ((\iALU|Add0~30\) # (GND)))))
-- \iALU|Add0~35\ = CARRY((\iALU|Add0~33_combout\ & (!\MUXA|oQ[6]~22_combout\ & !\iALU|Add0~30\)) # (!\iALU|Add0~33_combout\ & ((!\iALU|Add0~30\) # (!\MUXA|oQ[6]~22_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~33_combout\,
	datab => \MUXA|oQ[6]~22_combout\,
	datad => VCC,
	cin => \iALU|Add0~30\,
	combout => \iALU|Add0~34_combout\,
	cout => \iALU|Add0~35\);

-- Location: LCCOMB_X32_Y16_N26
\iALU|Add0~37\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~37_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~34_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~36_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001000100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datab => \iALU|Add0~36_combout\,
	datad => \iALU|Add0~34_combout\,
	combout => \iALU|Add0~37_combout\);

-- Location: FF_X32_Y16_N27
\R3|sREG[6]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~37_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(6));

-- Location: LCCOMB_X32_Y17_N20
\MUXB|oQ[6]~27\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[6]~27_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(6))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(6)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R2|sREG\(6),
	datac => \R0|sREG\(6),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[6]~27_combout\);

-- Location: LCCOMB_X32_Y16_N20
\MUXB|oQ[6]~28\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[6]~28_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[6]~27_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[6]~27_combout\ & (\R3|sREG\(6))) # (!\MUXB|oQ[6]~27_combout\ & ((\R1|sREG\(6))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001010000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R3|sREG\(6),
	datac => \R1|sREG\(6),
	datad => \MUXB|oQ[6]~27_combout\,
	combout => \MUXB|oQ[6]~28_combout\);

-- Location: LCCOMB_X32_Y16_N8
\iALU|Add0~33\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~33_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[6]~28_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000110111011000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \iCU|WideOr6~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[6]~28_combout\,
	combout => \iALU|Add0~33_combout\);

-- Location: LCCOMB_X33_Y16_N0
\iALU|Add0~39\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~39_combout\ = ((\iALU|Add0~38_combout\ $ (\MUXA|oQ[7]~25_combout\ $ (!\iALU|Add0~35\)))) # (GND)
-- \iALU|Add0~40\ = CARRY((\iALU|Add0~38_combout\ & ((\MUXA|oQ[7]~25_combout\) # (!\iALU|Add0~35\))) # (!\iALU|Add0~38_combout\ & (\MUXA|oQ[7]~25_combout\ & !\iALU|Add0~35\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~38_combout\,
	datab => \MUXA|oQ[7]~25_combout\,
	datad => VCC,
	cin => \iALU|Add0~35\,
	combout => \iALU|Add0~39_combout\,
	cout => \iALU|Add0~40\);

-- Location: LCCOMB_X32_Y16_N12
\iALU|Add0~41\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~41_combout\ = (\iCU|WideOr4~combout\ & ((\iALU|Mux12~1_combout\ & ((!\MUXA|oQ[7]~25_combout\))) # (!\iALU|Mux12~1_combout\ & ((\MUXB|oQ[7]~30_combout\) # (\MUXA|oQ[7]~25_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[7]~25_combout\ & 
-- ((\MUXB|oQ[7]~30_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011111011001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[7]~30_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \iALU|Mux12~1_combout\,
	datad => \MUXA|oQ[7]~25_combout\,
	combout => \iALU|Add0~41_combout\);

-- Location: LCCOMB_X32_Y16_N24
\iALU|Add0~42\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~42_combout\ = (\iALU|Mux12~0_combout\ & (\iALU|Add0~39_combout\)) # (!\iALU|Mux12~0_combout\ & ((\iALU|Add0~41_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111010110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~39_combout\,
	datad => \iALU|Add0~41_combout\,
	combout => \iALU|Add0~42_combout\);

-- Location: FF_X32_Y16_N25
\R3|sREG[7]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~42_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(7));

-- Location: LCCOMB_X33_Y18_N8
\MUXB|oQ[7]~29\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[7]~29_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(7) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(7)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000111111001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(7),
	datab => \R0|sREG\(7),
	datac => \MUXB|oQ[1]~13_combout\,
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[7]~29_combout\);

-- Location: LCCOMB_X32_Y16_N10
\MUXB|oQ[7]~30\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[7]~30_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[7]~29_combout\ & (\R3|sREG\(7))) # (!\MUXB|oQ[7]~29_combout\ & ((\R2|sREG\(7)))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[7]~29_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~14_combout\,
	datab => \R3|sREG\(7),
	datac => \R2|sREG\(7),
	datad => \MUXB|oQ[7]~29_combout\,
	combout => \MUXB|oQ[7]~30_combout\);

-- Location: LCCOMB_X32_Y16_N16
\iALU|Add0~38\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~38_combout\ = (\iCU|WideOr4~combout\ & (((\iCU|WideOr6~combout\)))) # (!\iCU|WideOr4~combout\ & (\iCU|WideOr5~combout\ $ (((\MUXB|oQ[7]~30_combout\)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100010111001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr5~combout\,
	datab => \iCU|WideOr6~combout\,
	datac => \iCU|WideOr4~combout\,
	datad => \MUXB|oQ[7]~30_combout\,
	combout => \iALU|Add0~38_combout\);

-- Location: LCCOMB_X33_Y16_N2
\iALU|Add0~44\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~44_combout\ = (\MUXA|oQ[8]~28_combout\ & ((\iALU|Add0~43_combout\ & (\iALU|Add0~40\ & VCC)) # (!\iALU|Add0~43_combout\ & (!\iALU|Add0~40\)))) # (!\MUXA|oQ[8]~28_combout\ & ((\iALU|Add0~43_combout\ & (!\iALU|Add0~40\)) # (!\iALU|Add0~43_combout\ 
-- & ((\iALU|Add0~40\) # (GND)))))
-- \iALU|Add0~45\ = CARRY((\MUXA|oQ[8]~28_combout\ & (!\iALU|Add0~43_combout\ & !\iALU|Add0~40\)) # (!\MUXA|oQ[8]~28_combout\ & ((!\iALU|Add0~40\) # (!\iALU|Add0~43_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[8]~28_combout\,
	datab => \iALU|Add0~43_combout\,
	datad => VCC,
	cin => \iALU|Add0~40\,
	combout => \iALU|Add0~44_combout\,
	cout => \iALU|Add0~45\);

-- Location: LCCOMB_X32_Y16_N4
\iALU|Add0~47\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~47_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~44_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~46_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111101001010000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~46_combout\,
	datad => \iALU|Add0~44_combout\,
	combout => \iALU|Add0~47_combout\);

-- Location: FF_X32_Y16_N5
\R3|sREG[8]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~47_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(8));

-- Location: LCCOMB_X31_Y16_N10
\MUXA|oQ[8]~26\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[8]~26_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(8)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(8)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(8),
	datab => \R2|sREG\(8),
	datac => \iCU|WideOr3~combout\,
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[8]~26_combout\);

-- Location: LCCOMB_X31_Y16_N16
\MUXA|oQ[8]~27\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[8]~27_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[8]~26_combout\ & (\R3|sREG\(8))) # (!\MUXA|oQ[8]~26_combout\ & ((\R1|sREG\(8)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[8]~26_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010111111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(8),
	datab => \R1|sREG\(8),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[8]~26_combout\,
	combout => \MUXA|oQ[8]~27_combout\);

-- Location: LCCOMB_X31_Y16_N6
\MUXA|oQ[8]~28\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[8]~28_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[8]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[8]~27_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iDATA[8]~input_o\,
	datab => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[8]~27_combout\,
	combout => \MUXA|oQ[8]~28_combout\);

-- Location: LCCOMB_X33_Y16_N4
\iALU|Add0~49\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~49_combout\ = ((\iALU|Add0~48_combout\ $ (\MUXA|oQ[9]~31_combout\ $ (!\iALU|Add0~45\)))) # (GND)
-- \iALU|Add0~50\ = CARRY((\iALU|Add0~48_combout\ & ((\MUXA|oQ[9]~31_combout\) # (!\iALU|Add0~45\))) # (!\iALU|Add0~48_combout\ & (\MUXA|oQ[9]~31_combout\ & !\iALU|Add0~45\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~48_combout\,
	datab => \MUXA|oQ[9]~31_combout\,
	datad => VCC,
	cin => \iALU|Add0~45\,
	combout => \iALU|Add0~49_combout\,
	cout => \iALU|Add0~50\);

-- Location: LCCOMB_X32_Y18_N4
\iALU|Add0~51\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~51_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[9]~31_combout\ & ((!\iALU|Mux12~1_combout\))) # (!\MUXA|oQ[9]~31_combout\ & ((\MUXB|oQ[9]~34_combout\) # (\iALU|Mux12~1_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[9]~31_combout\ & 
-- ((\MUXB|oQ[9]~34_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011110011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[9]~34_combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \MUXA|oQ[9]~31_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~51_combout\);

-- Location: LCCOMB_X33_Y16_N24
\iALU|Add0~52\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~52_combout\ = (\iALU|Mux12~0_combout\ & (\iALU|Add0~49_combout\)) # (!\iALU|Mux12~0_combout\ & ((\iALU|Add0~51_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datab => \iALU|Add0~49_combout\,
	datad => \iALU|Add0~51_combout\,
	combout => \iALU|Add0~52_combout\);

-- Location: FF_X33_Y18_N3
\R2|sREG[9]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~52_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr5~1_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R2|sREG\(9));

-- Location: LCCOMB_X33_Y18_N24
\MUXB|oQ[9]~33\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[9]~33_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\R0|sREG\(9) & !\MUXB|oQ[1]~14_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\R1|sREG\(9)) # ((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011001111100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(9),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(9),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[9]~33_combout\);

-- Location: LCCOMB_X32_Y18_N18
\MUXB|oQ[9]~34\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[9]~34_combout\ = (\MUXB|oQ[1]~14_combout\ & ((\MUXB|oQ[9]~33_combout\ & ((\R3|sREG\(9)))) # (!\MUXB|oQ[9]~33_combout\ & (\R2|sREG\(9))))) # (!\MUXB|oQ[1]~14_combout\ & (((\MUXB|oQ[9]~33_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111001110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(9),
	datab => \MUXB|oQ[1]~14_combout\,
	datac => \R3|sREG\(9),
	datad => \MUXB|oQ[9]~33_combout\,
	combout => \MUXB|oQ[9]~34_combout\);

-- Location: LCCOMB_X32_Y18_N20
\iALU|Add0~48\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~48_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[9]~34_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000101110111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr6~combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[9]~34_combout\,
	combout => \iALU|Add0~48_combout\);

-- Location: LCCOMB_X33_Y16_N6
\iALU|Add0~54\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~54_combout\ = (\MUXA|oQ[10]~34_combout\ & ((\iALU|Add0~53_combout\ & (\iALU|Add0~50\ & VCC)) # (!\iALU|Add0~53_combout\ & (!\iALU|Add0~50\)))) # (!\MUXA|oQ[10]~34_combout\ & ((\iALU|Add0~53_combout\ & (!\iALU|Add0~50\)) # 
-- (!\iALU|Add0~53_combout\ & ((\iALU|Add0~50\) # (GND)))))
-- \iALU|Add0~55\ = CARRY((\MUXA|oQ[10]~34_combout\ & (!\iALU|Add0~53_combout\ & !\iALU|Add0~50\)) # (!\MUXA|oQ[10]~34_combout\ & ((!\iALU|Add0~50\) # (!\iALU|Add0~53_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[10]~34_combout\,
	datab => \iALU|Add0~53_combout\,
	datad => VCC,
	cin => \iALU|Add0~50\,
	combout => \iALU|Add0~54_combout\,
	cout => \iALU|Add0~55\);

-- Location: LCCOMB_X33_Y16_N20
\iALU|Add0~57\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~57_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~54_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~56_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~56_combout\,
	datab => \iALU|Mux12~0_combout\,
	datad => \iALU|Add0~54_combout\,
	combout => \iALU|Add0~57_combout\);

-- Location: FF_X33_Y16_N31
\R3|sREG[10]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~57_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(10));

-- Location: LCCOMB_X32_Y18_N12
\MUXA|oQ[10]~32\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[10]~32_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(10)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(10)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(10),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(10),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[10]~32_combout\);

-- Location: LCCOMB_X32_Y18_N28
\MUXA|oQ[10]~33\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[10]~33_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[10]~32_combout\ & (\R3|sREG\(10))) # (!\MUXA|oQ[10]~32_combout\ & ((\R1|sREG\(10)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[10]~32_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(10),
	datab => \iCU|WideOr3~combout\,
	datac => \R1|sREG\(10),
	datad => \MUXA|oQ[10]~32_combout\,
	combout => \MUXA|oQ[10]~33_combout\);

-- Location: LCCOMB_X32_Y18_N14
\MUXA|oQ[10]~34\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[10]~34_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[10]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[10]~33_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iDATA[10]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[10]~33_combout\,
	combout => \MUXA|oQ[10]~34_combout\);

-- Location: LCCOMB_X33_Y16_N8
\iALU|Add0~59\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~59_combout\ = ((\MUXA|oQ[11]~37_combout\ $ (\iALU|Add0~58_combout\ $ (!\iALU|Add0~55\)))) # (GND)
-- \iALU|Add0~60\ = CARRY((\MUXA|oQ[11]~37_combout\ & ((\iALU|Add0~58_combout\) # (!\iALU|Add0~55\))) # (!\MUXA|oQ[11]~37_combout\ & (\iALU|Add0~58_combout\ & !\iALU|Add0~55\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[11]~37_combout\,
	datab => \iALU|Add0~58_combout\,
	datad => VCC,
	cin => \iALU|Add0~55\,
	combout => \iALU|Add0~59_combout\,
	cout => \iALU|Add0~60\);

-- Location: LCCOMB_X33_Y16_N18
\iALU|Add0~62\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~62_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~59_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~61_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~61_combout\,
	datab => \iALU|Mux12~0_combout\,
	datad => \iALU|Add0~59_combout\,
	combout => \iALU|Add0~62_combout\);

-- Location: FF_X33_Y16_N21
\R3|sREG[11]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~62_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(11));

-- Location: LCCOMB_X33_Y18_N22
\MUXA|oQ[11]~35\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[11]~35_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(11)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(11)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(11),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(11),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[11]~35_combout\);

-- Location: LCCOMB_X32_Y18_N24
\MUXA|oQ[11]~36\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[11]~36_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[11]~35_combout\ & (\R3|sREG\(11))) # (!\MUXA|oQ[11]~35_combout\ & ((\R1|sREG\(11)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[11]~35_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R3|sREG\(11),
	datab => \iCU|WideOr3~combout\,
	datac => \R1|sREG\(11),
	datad => \MUXA|oQ[11]~35_combout\,
	combout => \MUXA|oQ[11]~36_combout\);

-- Location: LCCOMB_X32_Y18_N30
\MUXA|oQ[11]~37\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[11]~37_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[11]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[11]~36_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iDATA[11]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[11]~36_combout\,
	combout => \MUXA|oQ[11]~37_combout\);

-- Location: LCCOMB_X33_Y16_N10
\iALU|Add0~64\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~64_combout\ = (\iALU|Add0~63_combout\ & ((\MUXA|oQ[12]~40_combout\ & (\iALU|Add0~60\ & VCC)) # (!\MUXA|oQ[12]~40_combout\ & (!\iALU|Add0~60\)))) # (!\iALU|Add0~63_combout\ & ((\MUXA|oQ[12]~40_combout\ & (!\iALU|Add0~60\)) # 
-- (!\MUXA|oQ[12]~40_combout\ & ((\iALU|Add0~60\) # (GND)))))
-- \iALU|Add0~65\ = CARRY((\iALU|Add0~63_combout\ & (!\MUXA|oQ[12]~40_combout\ & !\iALU|Add0~60\)) # (!\iALU|Add0~63_combout\ & ((!\iALU|Add0~60\) # (!\MUXA|oQ[12]~40_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~63_combout\,
	datab => \MUXA|oQ[12]~40_combout\,
	datad => VCC,
	cin => \iALU|Add0~60\,
	combout => \iALU|Add0~64_combout\,
	cout => \iALU|Add0~65\);

-- Location: LCCOMB_X33_Y16_N22
\iALU|Add0~67\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~67_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~64_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~66_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~66_combout\,
	datad => \iALU|Add0~64_combout\,
	combout => \iALU|Add0~67_combout\);

-- Location: FF_X33_Y16_N23
\R3|sREG[12]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~67_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(12));

-- Location: LCCOMB_X33_Y18_N16
\MUXB|oQ[12]~39\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[12]~39_combout\ = (\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[1]~14_combout\ & (\R2|sREG\(12))) # (!\MUXB|oQ[1]~14_combout\ & ((\R0|sREG\(12)))))) # (!\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[1]~14_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(12),
	datab => \MUXB|oQ[1]~13_combout\,
	datac => \R0|sREG\(12),
	datad => \MUXB|oQ[1]~14_combout\,
	combout => \MUXB|oQ[12]~39_combout\);

-- Location: LCCOMB_X32_Y18_N16
\MUXB|oQ[12]~40\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXB|oQ[12]~40_combout\ = (\MUXB|oQ[1]~13_combout\ & (((\MUXB|oQ[12]~39_combout\)))) # (!\MUXB|oQ[1]~13_combout\ & ((\MUXB|oQ[12]~39_combout\ & (\R3|sREG\(12))) # (!\MUXB|oQ[12]~39_combout\ & ((\R1|sREG\(12))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001010000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXB|oQ[1]~13_combout\,
	datab => \R3|sREG\(12),
	datac => \R1|sREG\(12),
	datad => \MUXB|oQ[12]~39_combout\,
	combout => \MUXB|oQ[12]~40_combout\);

-- Location: LCCOMB_X32_Y18_N2
\iALU|Add0~63\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~63_combout\ = (\iCU|WideOr4~combout\ & (\iCU|WideOr6~combout\)) # (!\iCU|WideOr4~combout\ & ((\iCU|WideOr5~combout\ $ (\MUXB|oQ[12]~40_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000101110111000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr6~combout\,
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \MUXB|oQ[12]~40_combout\,
	combout => \iALU|Add0~63_combout\);

-- Location: LCCOMB_X33_Y16_N12
\iALU|Add0~69\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~69_combout\ = ((\MUXA|oQ[13]~43_combout\ $ (\iALU|Add0~68_combout\ $ (!\iALU|Add0~65\)))) # (GND)
-- \iALU|Add0~70\ = CARRY((\MUXA|oQ[13]~43_combout\ & ((\iALU|Add0~68_combout\) # (!\iALU|Add0~65\))) # (!\MUXA|oQ[13]~43_combout\ & (\iALU|Add0~68_combout\ & !\iALU|Add0~65\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0110100110001110",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[13]~43_combout\,
	datab => \iALU|Add0~68_combout\,
	datad => VCC,
	cin => \iALU|Add0~65\,
	combout => \iALU|Add0~69_combout\,
	cout => \iALU|Add0~70\);

-- Location: LCCOMB_X33_Y16_N30
\iALU|Add0~72\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~72_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~69_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~71_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001000100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datab => \iALU|Add0~71_combout\,
	datad => \iALU|Add0~69_combout\,
	combout => \iALU|Add0~72_combout\);

-- Location: FF_X33_Y16_N25
\R1|sREG[13]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~72_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(13));

-- Location: LCCOMB_X31_Y16_N24
\MUXA|oQ[13]~41\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[13]~41_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(13)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(13)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(13),
	datab => \R2|sREG\(13),
	datac => \iCU|WideOr3~combout\,
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[13]~41_combout\);

-- Location: LCCOMB_X31_Y16_N2
\MUXA|oQ[13]~42\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[13]~42_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[13]~41_combout\ & ((\R3|sREG\(13)))) # (!\MUXA|oQ[13]~41_combout\ & (\R1|sREG\(13))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[13]~41_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(13),
	datab => \R3|sREG\(13),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[13]~41_combout\,
	combout => \MUXA|oQ[13]~42_combout\);

-- Location: LCCOMB_X31_Y16_N20
\MUXA|oQ[13]~43\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[13]~43_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[13]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[13]~42_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111111000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iDATA[13]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[13]~42_combout\,
	combout => \MUXA|oQ[13]~43_combout\);

-- Location: LCCOMB_X33_Y16_N14
\iALU|Add0~74\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~74_combout\ = (\MUXA|oQ[14]~46_combout\ & ((\iALU|Add0~73_combout\ & (\iALU|Add0~70\ & VCC)) # (!\iALU|Add0~73_combout\ & (!\iALU|Add0~70\)))) # (!\MUXA|oQ[14]~46_combout\ & ((\iALU|Add0~73_combout\ & (!\iALU|Add0~70\)) # 
-- (!\iALU|Add0~73_combout\ & ((\iALU|Add0~70\) # (GND)))))
-- \iALU|Add0~75\ = CARRY((\MUXA|oQ[14]~46_combout\ & (!\iALU|Add0~73_combout\ & !\iALU|Add0~70\)) # (!\MUXA|oQ[14]~46_combout\ & ((!\iALU|Add0~70\) # (!\iALU|Add0~73_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1001011000010111",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[14]~46_combout\,
	datab => \iALU|Add0~73_combout\,
	datad => VCC,
	cin => \iALU|Add0~70\,
	combout => \iALU|Add0~74_combout\,
	cout => \iALU|Add0~75\);

-- Location: LCCOMB_X33_Y16_N28
\iALU|Add0~77\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~77_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~74_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~76_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111001000100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Mux12~0_combout\,
	datab => \iALU|Add0~76_combout\,
	datad => \iALU|Add0~74_combout\,
	combout => \iALU|Add0~77_combout\);

-- Location: FF_X33_Y16_N5
\R1|sREG[14]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iALU|Add0~77_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	ena => \iCU|oREG_WE[1]~0_combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R1|sREG\(14));

-- Location: LCCOMB_X32_Y17_N6
\MUXA|oQ[14]~44\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[14]~44_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & ((\R2|sREG\(14)))) # (!\iCU|WideOr2~combout\ & (\R0|sREG\(14)))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R0|sREG\(14),
	datab => \iCU|WideOr3~combout\,
	datac => \R2|sREG\(14),
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[14]~44_combout\);

-- Location: LCCOMB_X34_Y16_N28
\MUXA|oQ[14]~45\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[14]~45_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[14]~44_combout\ & ((\R3|sREG\(14)))) # (!\MUXA|oQ[14]~44_combout\ & (\R1|sREG\(14))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[14]~44_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R1|sREG\(14),
	datab => \R3|sREG\(14),
	datac => \iCU|WideOr3~combout\,
	datad => \MUXA|oQ[14]~44_combout\,
	combout => \MUXA|oQ[14]~45_combout\);

-- Location: LCCOMB_X34_Y16_N30
\MUXA|oQ[14]~46\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[14]~46_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[14]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[14]~45_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010111110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iDATA[14]~input_o\,
	datac => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[14]~45_combout\,
	combout => \MUXA|oQ[14]~46_combout\);

-- Location: LCCOMB_X33_Y16_N16
\iALU|Add0~79\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~79_combout\ = \MUXA|oQ[15]~49_combout\ $ (\iALU|Add0~75\ $ (!\iALU|Add0~78_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0011110011000011",
	sum_lutc_input => "cin")
-- pragma translate_on
PORT MAP (
	datab => \MUXA|oQ[15]~49_combout\,
	datad => \iALU|Add0~78_combout\,
	cin => \iALU|Add0~75\,
	combout => \iALU|Add0~79_combout\);

-- Location: LCCOMB_X34_Y16_N22
\iCU|Selector0~8\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~8_combout\ = (\iALU|Mux12~0_combout\ & (((!\iALU|Add0~39_combout\)))) # (!\iALU|Mux12~0_combout\ & (!\iALU|Add0~41_combout\ & ((!\iALU|Add0~81_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000110000011101",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~41_combout\,
	datab => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~39_combout\,
	datad => \iALU|Add0~81_combout\,
	combout => \iCU|Selector0~8_combout\);

-- Location: LCCOMB_X34_Y16_N10
\iCU|Selector0~5\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~5_combout\ = (\iCU|Selector0~8_combout\ & (!\iALU|Add0~77_combout\ & ((!\iALU|Mux12~0_combout\) # (!\iALU|Add0~79_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000001110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~79_combout\,
	datab => \iALU|Mux12~0_combout\,
	datac => \iCU|Selector0~8_combout\,
	datad => \iALU|Add0~77_combout\,
	combout => \iCU|Selector0~5_combout\);

-- Location: LCCOMB_X34_Y16_N14
\iCU|Selector0~2\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~2_combout\ = (!\iALU|Add0~12_combout\ & (!\iALU|Add0~22_combout\ & (!\iALU|Add0~17_combout\ & !\iALU|Add0~27_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000000001",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~12_combout\,
	datab => \iALU|Add0~22_combout\,
	datac => \iALU|Add0~17_combout\,
	datad => \iALU|Add0~27_combout\,
	combout => \iCU|Selector0~2_combout\);

-- Location: LCCOMB_X33_Y16_N26
\iCU|Selector0~4\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~4_combout\ = (!\iALU|Add0~72_combout\ & (!\iALU|Add0~57_combout\ & (!\iALU|Add0~67_combout\ & !\iALU|Add0~62_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000000001",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~72_combout\,
	datab => \iALU|Add0~57_combout\,
	datac => \iALU|Add0~67_combout\,
	datad => \iALU|Add0~62_combout\,
	combout => \iCU|Selector0~4_combout\);

-- Location: LCCOMB_X34_Y16_N12
\iCU|Selector0~3\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~3_combout\ = (!\iALU|Add0~32_combout\ & (!\iALU|Add0~52_combout\ & (!\iALU|Add0~37_combout\ & !\iALU|Add0~47_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000000001",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~32_combout\,
	datab => \iALU|Add0~52_combout\,
	datac => \iALU|Add0~37_combout\,
	datad => \iALU|Add0~47_combout\,
	combout => \iCU|Selector0~3_combout\);

-- Location: LCCOMB_X34_Y16_N0
\iCU|Selector0~6\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|Selector0~6_combout\ = (\iCU|Selector0~5_combout\ & (\iCU|Selector0~2_combout\ & (\iCU|Selector0~4_combout\ & \iCU|Selector0~3_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1000000000000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|Selector0~5_combout\,
	datab => \iCU|Selector0~2_combout\,
	datac => \iCU|Selector0~4_combout\,
	datad => \iCU|Selector0~3_combout\,
	combout => \iCU|Selector0~6_combout\);

-- Location: LCCOMB_X34_Y16_N4
\iCU|currentState.DONE~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|currentState.DONE~0_combout\ = (\iCU|currentState.DONE~q\) # ((\iCU|Selector0~7_combout\ & \iCU|Selector0~6_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111101011110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|Selector0~7_combout\,
	datac => \iCU|currentState.DONE~q\,
	datad => \iCU|Selector0~6_combout\,
	combout => \iCU|currentState.DONE~0_combout\);

-- Location: FF_X34_Y16_N5
\iCU|currentState.DONE\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|currentState.DONE~0_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.DONE~q\);

-- Location: LCCOMB_X30_Y17_N14
\iCU|WideOr5~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr5~0_combout\ = (\iCU|currentState.START~q\ & (!\iCU|currentState.T10~q\ & (!\iCU|currentState.T1~q\ & !\iCU|currentState.DONE~q\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000000000000010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.START~q\,
	datab => \iCU|currentState.T10~q\,
	datac => \iCU|currentState.T1~q\,
	datad => \iCU|currentState.DONE~q\,
	combout => \iCU|WideOr5~0_combout\);

-- Location: LCCOMB_X31_Y17_N0
\iCU|WideOr5\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr5~combout\ = (!\iCU|currentState.T3~q\ & (!\iCU|currentState.T6~q\ & \iCU|WideOr5~0_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000001100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T3~q\,
	datac => \iCU|currentState.T6~q\,
	datad => \iCU|WideOr5~0_combout\,
	combout => \iCU|WideOr5~combout\);

-- Location: LCCOMB_X31_Y16_N8
\iALU|Mux12~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Mux12~0_combout\ = \iCU|WideOr5~combout\ $ (((!\iCU|WideOr4~combout\ & \iCU|WideOr6~combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1100001111110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|WideOr4~combout\,
	datac => \iCU|WideOr5~combout\,
	datad => \iCU|WideOr6~combout\,
	combout => \iALU|Mux12~0_combout\);

-- Location: LCCOMB_X34_Y17_N16
\iALU|Add0~82\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~82_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~79_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~81_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111110000001100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iALU|Add0~81_combout\,
	datac => \iALU|Mux12~0_combout\,
	datad => \iALU|Add0~79_combout\,
	combout => \iALU|Add0~82_combout\);

-- Location: FF_X34_Y17_N17
\R3|sREG[15]\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iALU|Add0~82_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	ena => \iCU|WideOr2~combout\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \R3|sREG\(15));

-- Location: LCCOMB_X35_Y17_N6
\MUXA|oQ[15]~47\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[15]~47_combout\ = (\iCU|WideOr3~combout\ & (((\iCU|WideOr2~combout\)))) # (!\iCU|WideOr3~combout\ & ((\iCU|WideOr2~combout\ & (\R2|sREG\(15))) # (!\iCU|WideOr2~combout\ & ((\R0|sREG\(15))))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111101000001100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \R2|sREG\(15),
	datab => \R0|sREG\(15),
	datac => \iCU|WideOr3~combout\,
	datad => \iCU|WideOr2~combout\,
	combout => \MUXA|oQ[15]~47_combout\);

-- Location: LCCOMB_X34_Y17_N2
\MUXA|oQ[15]~48\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[15]~48_combout\ = (\iCU|WideOr3~combout\ & ((\MUXA|oQ[15]~47_combout\ & (\R3|sREG\(15))) # (!\MUXA|oQ[15]~47_combout\ & ((\R1|sREG\(15)))))) # (!\iCU|WideOr3~combout\ & (((\MUXA|oQ[15]~47_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1101110110100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr3~combout\,
	datab => \R3|sREG\(15),
	datac => \R1|sREG\(15),
	datad => \MUXA|oQ[15]~47_combout\,
	combout => \MUXA|oQ[15]~48_combout\);

-- Location: LCCOMB_X34_Y17_N12
\MUXA|oQ[15]~49\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \MUXA|oQ[15]~49_combout\ = (\iCU|currentState.T1~q\ & (\iDATA[15]~input_o\)) # (!\iCU|currentState.T1~q\ & ((\MUXA|oQ[15]~48_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1011101110001000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iDATA[15]~input_o\,
	datab => \iCU|currentState.T1~q\,
	datad => \MUXA|oQ[15]~48_combout\,
	combout => \MUXA|oQ[15]~49_combout\);

-- Location: LCCOMB_X34_Y17_N14
\iALU|Add0~81\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~81_combout\ = (\MUXA|oQ[15]~49_combout\ & ((\iCU|WideOr4~combout\ & ((!\iALU|Mux12~1_combout\))) # (!\iCU|WideOr4~combout\ & ((\MUXB|oQ[15]~46_combout\) # (\iALU|Mux12~1_combout\))))) # (!\MUXA|oQ[15]~49_combout\ & (\iCU|WideOr4~combout\ & 
-- ((\MUXB|oQ[15]~46_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0101101011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \MUXA|oQ[15]~49_combout\,
	datab => \MUXB|oQ[15]~46_combout\,
	datac => \iCU|WideOr4~combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~81_combout\);

-- Location: LCCOMB_X33_Y17_N14
\iCU|nextState.T8~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|nextState.T8~0_combout\ = (\iCU|currentState.T5~q\ & ((\iALU|Mux12~0_combout\ & ((!\iALU|Add0~79_combout\))) # (!\iALU|Mux12~0_combout\ & (!\iALU|Add0~81_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0000110001000100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~81_combout\,
	datab => \iCU|currentState.T5~q\,
	datac => \iALU|Add0~79_combout\,
	datad => \iALU|Mux12~0_combout\,
	combout => \iCU|nextState.T8~0_combout\);

-- Location: FF_X33_Y17_N15
\iCU|currentState.T8\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|nextState.T8~0_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T8~q\);

-- Location: FF_X33_Y17_N9
\iCU|currentState.T9\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iCU|currentState.T8~q\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T9~q\);

-- Location: LCCOMB_X30_Y17_N24
\iCU|currentState.T10~feeder\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|currentState.T10~feeder_combout\ = \iCU|currentState.T9~q\

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111100000000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datad => \iCU|currentState.T9~q\,
	combout => \iCU|currentState.T10~feeder_combout\);

-- Location: FF_X30_Y17_N25
\iCU|currentState.T10\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|currentState.T10~feeder_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T10~q\);

-- Location: LCCOMB_X30_Y17_N20
\iCU|nextState.T3\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|nextState.T3~combout\ = (\iCU|currentState.T2~q\) # (\iCU|currentState.T10~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111110000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datac => \iCU|currentState.T2~q\,
	datad => \iCU|currentState.T10~q\,
	combout => \iCU|nextState.T3~combout\);

-- Location: FF_X30_Y17_N21
\iCU|currentState.T3\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|nextState.T3~combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T3~q\);

-- Location: LCCOMB_X34_Y16_N8
\iCU|nextState.T4~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|nextState.T4~0_combout\ = (\iCU|currentState.T3~q\ & ((\iALU|Add0~7_combout\) # (!\iCU|Selector0~6_combout\)))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010000010101010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T3~q\,
	datac => \iALU|Add0~7_combout\,
	datad => \iCU|Selector0~6_combout\,
	combout => \iCU|nextState.T4~0_combout\);

-- Location: FF_X34_Y16_N9
\iCU|currentState.T4\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|nextState.T4~0_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T4~q\);

-- Location: LCCOMB_X33_Y17_N4
\iCU|nextState.T5\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|nextState.T5~combout\ = (\iCU|currentState.T7~q\) # (\iCU|currentState.T4~q\)

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111001100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T7~q\,
	datad => \iCU|currentState.T4~q\,
	combout => \iCU|nextState.T5~combout\);

-- Location: FF_X33_Y17_N5
\iCU|currentState.T5\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|nextState.T5~combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T5~q\);

-- Location: LCCOMB_X30_Y17_N6
\iCU|nextState.T6~0\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|nextState.T6~0_combout\ = (\iCU|currentState.T5~q\ & ((\iALU|Mux12~0_combout\ & ((\iALU|Add0~79_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~81_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1010100000100000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|currentState.T5~q\,
	datab => \iALU|Mux12~0_combout\,
	datac => \iALU|Add0~81_combout\,
	datad => \iALU|Add0~79_combout\,
	combout => \iCU|nextState.T6~0_combout\);

-- Location: FF_X30_Y17_N7
\iCU|currentState.T6\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	d => \iCU|nextState.T6~0_combout\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T6~q\);

-- Location: FF_X33_Y17_N3
\iCU|currentState.T7\ : dffeas
-- pragma translate_off
GENERIC MAP (
	is_wysiwyg => "true",
	power_up => "low")
-- pragma translate_on
PORT MAP (
	clk => \iCLK~inputclkctrl_outclk\,
	asdata => \iCU|currentState.T6~q\,
	clrn => \ALT_INV_iRST~inputclkctrl_outclk\,
	sload => VCC,
	devclrn => ww_devclrn,
	devpor => ww_devpor,
	q => \iCU|currentState.T7~q\);

-- Location: LCCOMB_X30_Y17_N18
\iCU|WideOr4\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iCU|WideOr4~combout\ = (\iCU|currentState.T2~q\) # ((\iCU|currentState.T7~q\) # (\iCU|currentState.T9~q\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1111111111111100",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	datab => \iCU|currentState.T2~q\,
	datac => \iCU|currentState.T7~q\,
	datad => \iCU|currentState.T9~q\,
	combout => \iCU|WideOr4~combout\);

-- Location: LCCOMB_X34_Y17_N10
\iALU|Add0~6\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~6_combout\ = (\iCU|WideOr4~combout\ & ((\MUXA|oQ[0]~4_combout\ & ((!\iALU|Mux12~1_combout\))) # (!\MUXA|oQ[0]~4_combout\ & ((\MUXB|oQ[0]~16_combout\) # (\iALU|Mux12~1_combout\))))) # (!\iCU|WideOr4~combout\ & (\MUXA|oQ[0]~4_combout\ & 
-- ((\MUXB|oQ[0]~16_combout\) # (\iALU|Mux12~1_combout\))))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "0101101011101000",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iCU|WideOr4~combout\,
	datab => \MUXB|oQ[0]~16_combout\,
	datac => \MUXA|oQ[0]~4_combout\,
	datad => \iALU|Mux12~1_combout\,
	combout => \iALU|Add0~6_combout\);

-- Location: LCCOMB_X34_Y17_N24
\iALU|Add0~7\ : fiftyfivenm_lcell_comb
-- Equation(s):
-- \iALU|Add0~7_combout\ = (\iALU|Mux12~0_combout\ & ((\iALU|Add0~4_combout\))) # (!\iALU|Mux12~0_combout\ & (\iALU|Add0~6_combout\))

-- pragma translate_off
GENERIC MAP (
	lut_mask => "1110111000100010",
	sum_lutc_input => "datac")
-- pragma translate_on
PORT MAP (
	dataa => \iALU|Add0~6_combout\,
	datab => \iALU|Mux12~0_combout\,
	datad => \iALU|Add0~4_combout\,
	combout => \iALU|Add0~7_combout\);

-- Location: UNVM_X0_Y18_N40
\~QUARTUS_CREATED_UNVM~\ : fiftyfivenm_unvm
-- pragma translate_off
GENERIC MAP (
	addr_range1_end_addr => -1,
	addr_range1_offset => -1,
	addr_range2_offset => -1,
	is_compressed_image => "false",
	is_dual_boot => "false",
	is_eram_skip => "false",
	max_ufm_valid_addr => -1,
	max_valid_addr => -1,
	min_ufm_valid_addr => -1,
	min_valid_addr => -1,
	part_name => "quartus_created_unvm",
	reserve_block => "true")
-- pragma translate_on
PORT MAP (
	nosc_ena => \~QUARTUS_CREATED_GND~I_combout\,
	xe_ye => \~QUARTUS_CREATED_GND~I_combout\,
	se => \~QUARTUS_CREATED_GND~I_combout\,
	busy => \~QUARTUS_CREATED_UNVM~~busy\);

-- Location: ADCBLOCK_X25_Y28_N0
\~QUARTUS_CREATED_ADC1~\ : fiftyfivenm_adcblock
-- pragma translate_off
GENERIC MAP (
	analog_input_pin_mask => 0,
	clkdiv => 1,
	device_partname_fivechar_prefix => "none",
	is_this_first_or_second_adc => 1,
	prescalar => 0,
	pwd => 1,
	refsel => 0,
	reserve_block => "true",
	testbits => 66,
	tsclkdiv => 1,
	tsclksel => 0)
-- pragma translate_on
PORT MAP (
	soc => \~QUARTUS_CREATED_GND~I_combout\,
	usr_pwd => VCC,
	tsen => \~QUARTUS_CREATED_GND~I_combout\,
	chsel => \~QUARTUS_CREATED_ADC1~_CHSEL_bus\,
	eoc => \~QUARTUS_CREATED_ADC1~~eoc\);

ww_oDATA(0) <= \oDATA[0]~output_o\;

ww_oDATA(1) <= \oDATA[1]~output_o\;

ww_oDATA(2) <= \oDATA[2]~output_o\;

ww_oDATA(3) <= \oDATA[3]~output_o\;

ww_oDATA(4) <= \oDATA[4]~output_o\;

ww_oDATA(5) <= \oDATA[5]~output_o\;

ww_oDATA(6) <= \oDATA[6]~output_o\;

ww_oDATA(7) <= \oDATA[7]~output_o\;

ww_oDATA(8) <= \oDATA[8]~output_o\;

ww_oDATA(9) <= \oDATA[9]~output_o\;

ww_oDATA(10) <= \oDATA[10]~output_o\;

ww_oDATA(11) <= \oDATA[11]~output_o\;

ww_oDATA(12) <= \oDATA[12]~output_o\;

ww_oDATA(13) <= \oDATA[13]~output_o\;

ww_oDATA(14) <= \oDATA[14]~output_o\;

ww_oDATA(15) <= \oDATA[15]~output_o\;
END structure;


