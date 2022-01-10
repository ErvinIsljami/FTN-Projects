CREATE TABLE parking(
	 key integer NOT NULL,
	 evidencioni_broj varchar(10) NOT NULL,
	 registarska_oznaka varchar(11) NOT NULL,
     datum_vreme varchar(21) NOT NULL,
     parking_mesto varchar(7) NOT NULL,
     boravak_minuta integer NOT NULL,
	 CONSTRAINT parking_pk PRIMARY KEY (key)
);

insert into parking values (123430, '123456789', 'ns-1234-ns', '2020.02.20.#20:20', 'pamesto', 125);
insert into parking values (345123, '635464321', 'ns-2345-ns', '2019.04.10.#13:44', 'ffmesto', 300);
insert into parking values (100011, '865473764', 'ns-6458-ns', '2019.11.17.#21:32', 'remesto', 534);
insert into parking values (100012, '234523234', 'ns-8672-ns', '2018.08.25.#14:21', 'trmesto', 867);
insert into parking values (100013, '645645232', 'ns-5355-ns', '2019.01.04.#11:15', 'uymesto', 978);
insert into parking values (412343, '674564565', 'ns-3333-ns', '2019.05.22.#16:37', 'qqmesto', 144);
insert into parking values (100014, '645243211', 'ns-6666-ns', '2019.01.04.#08:55', 'fgmesto', 328);


commit;


