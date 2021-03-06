transcript on
if {[file exists rtl_work]} {
	vdel -lib rtl_work -all
}
vlib rtl_work
vmap work rtl_work

vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/reg.vhd}
vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/mux.vhd}
vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/alu.vhd}
vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/cu.vhd}
vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/cpu_top.vhd}
vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/CntReg.vhd}

vcom -93 -work work {C:/Users/ervin/OneDrive/Desktop/lab8_postavka/simulation/modelsim/cpu_top_tb.vhd}

vsim -t 1ps -L altera -L lpm -L sgate -L altera_mf -L altera_lnsim -L fiftyfivenm -L rtl_work -L work -voptargs="+acc"  cpu_top_tb

add wave *
view structure
view signals
run 1 us
