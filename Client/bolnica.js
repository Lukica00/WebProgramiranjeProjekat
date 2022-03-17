import { adr, hideAll, err, show, fec } from "./include.js";
export class Bolnica {
    constructor(id, ime, lekari, brojMesta, sobe) {
        this.id = id;
        this.ime = ime;
        this.lekari = lekari;
        this.brojMesta = brojMesta;
        this.sobe = sobe;
        //fec("/Lecenje/DajBolesnike/" + this.id, "GET", sob => this.sobe = sob);
    }
    dajNezaposlene(sel) {
        fec("/Lekar/DajNezaposljeneLekare/" + this.id, "GET", lekari => {
            sel.replaceChildren();
            lekari.forEach(lekar => {
                const d = document.createElement("option");
                d.value = lekar.id;
                d.innerHTML = lekar.ime + " " + lekar.prezime;
                sel.appendChild(d);
            });
        });
    };
    dajZaposljene(element) {
        fec("/Lekar/DajZaposljeneLekare/" + this.id, "GET", lekari => {
            element.replaceChildren();
            this.lekari = [];
            lekari.forEach(lekar => {
                this.lekari.push(lekar);

                const tr = document.createElement("tr");
                let td = document.createElement("td");
                td.innerHTML = lekar.ime;
                tr.appendChild(td);

                td = document.createElement("td");
                td.innerHTML = lekar.prezime;
                tr.appendChild(td);

                td = document.createElement("td");
                td.innerHTML = lekar.pacijenti;
                tr.appendChild(td);

                td = document.createElement("td");
                const dugme = document.createElement("button");
                dugme.innerHTML = "Otpusti";
                dugme.addEventListener("click", () => fec("/Bolnica/OtpustiLekara/" + this.id + "/" + lekar.id, "PUt", () => {
                    this.dajZaposljene(element);
                    this.dajNezaposlene(document.getElementById("bolnicaZaposliIme"));
                }));
                td.appendChild(dugme);
                tr.appendChild(td);

                element.appendChild(tr);
            });
        });
    };
    draw() {
        let stranica = document.getElementById("bolnicaStranica");
        const tabela = document.getElementById("bolnicaTabela");
        const sobe = document.getElementById("bolnicaSobe");
        const bolovanja = document.getElementById("bolnicaBolovanja");
        sobe.replaceChildren();
        let h2 = stranica.querySelector("h2");
        h2.innerHTML = this.ime;

        let h3 = stranica.querySelector("h3");
        h3.innerHTML = "Broj mesta: " + this.brojMesta;

        document.getElementById("bolnicaPreimenuj").addEventListener("click", () => {
            let ime = document.getElementById("bolnicaIme").value;
            fetch(adr + "/Bolnica/PreimenujBolnicu/" + this.id + "/" + ime, { method: "PUT" }).then(() => {
                this.ime = ime;
                document.getElementById("meni" + this.id).innerHTML = this.ime;
                h2.innerHTML = this.ime;
            }, err)
        });

        document.getElementById("bolnicaObrisi").onclick = () => {
            fetch(adr + "/Bolnica/ObrisiBolnicu/" + this.id, { method: "DELETE" }).then(() => {
                document.getElementById("meni" + this.id).remove();
                hideAll();
            }, err);
        }
        const sel = document.getElementById("bolnicaZaposliIme");
        this.dajNezaposlene(sel);
        document.getElementById("bolnicaZaposli").onclick = () => {
            fetch(adr + "/Bolnica/ZaposliLekara/" + this.id + "/" + sel.value, { method: "POST" }).then(() => {
                this.draw();
            });
        };
        this.dajZaposljene(tabela);


        bolovanja.replaceChildren();
        let i = 1;
        const red = document.createElement("div");
        red.className = "redSoba";
        const dugme = document.createElement("button");
        dugme.innerHTML = "Soba ";
        dugme.id = "soba";
        let redTemp;

        while (i <= this.brojMesta) {
            if (i % 2) {
                if (redTemp) sobe.appendChild(redTemp);
                redTemp = red.cloneNode(true);
            }
            const dugmeTemp = dugme.cloneNode(true);
            dugmeTemp.innerHTML = dugme.innerHTML + i;
            dugmeTemp.id = dugme.id + i;
            dugmeTemp.onclick = () => {
                const idSobe = Number(dugmeTemp.id.substring(4));
                fec("/Lecenje/DajBolesnike/" + this.id + "/" + idSobe, "GET", (bolest) => {
                    bolovanja.replaceChildren();
                    if (bolest.length) {
                        const boles = bolest[0];
                        const p = document.createElement("p");
                        p.innerHTML = boles.pacijent.ime + " " + boles.pacijent.prezime + " " + boles.pacijent.jmbg + " " + boles.pocetak;
                        bolovanja.appendChild(p);
                        const dugmeZatvori = document.createElement("button");
                        dugmeZatvori.innerHTML = "Zatvori bolovanje";
                        dugmeZatvori.addEventListener("click", () => {
                            fec("/Lecenje/ZatvoriBolovanje/" + boles.id, "PUT", () => {
                                const q = this.sobe.indexOf(idSobe);
                                if (q > -1)
                                    this.sobe.splice(q, 1);
                                this.draw();
                            })
                        });
                        bolovanja.appendChild(dugmeZatvori);
                    } else {

                        const labela = document.createElement("label");
                        const selekt = document.createElement("select");
                        selekt.name = "lekarOdaberi";
                        selekt.id = "lekarOdaberi"
                        labela.for = "lekarOdaberi";
                        labela.innerHTML = "Zeljeni lekar:"
                        this.lekari.forEach(lekar => {
                            const option = document.createElement("option");
                            option.value = lekar.id;
                            option.innerHTML = lekar.ime + " " + lekar.prezime;
                            option.selected = true;
                            selekt.appendChild(option);
                        });


                        const labelaPacijent = document.createElement("label");
                        const selektPacijent = document.createElement("select");
                        selektPacijent.name = "pacijenOdaberi";
                        selektPacijent.id = "pacijenOdaberi"
                        labelaPacijent.for = "pacijentOdaberi";
                        labelaPacijent.innerHTML = "Pacijent:"

                        
                        const dugmeOtvori = document.createElement("button");

                        fec("/Pacijent/DajZdravePacijente", "GET", (pacijenti) => {
                            if (!pacijenti.length) dugmeOtvori.disabled = true;
                            pacijenti.forEach(pacijent => {
                                const option = document.createElement("option");
                                option.value = pacijent.id;
                                option.innerHTML = pacijent.ime + " " + pacijent.prezime;
                                option.selected = true;
                                selektPacijent.appendChild(option);
                            })
                        });

                        dugmeOtvori.innerHTML = "Otvori bolovanje";
                        dugmeOtvori.addEventListener("click", () => {
                            fec("/Lecenje/DodajBolovanje/" + selektPacijent.value + "/" + this.id + "/" + selekt.value + "/" + idSobe, "POST", () => {
                                this.sobe.push(idSobe);
                                this.draw();
                            })

                        });
                        bolovanja.appendChild(labela);
                        bolovanja.appendChild(selekt);
                        bolovanja.appendChild(labelaPacijent);
                        bolovanja.appendChild(selektPacijent);
                        bolovanja.appendChild(dugmeOtvori);
                    }
                });
            };
            redTemp.appendChild(dugmeTemp);
            i++;
        }
        sobe.appendChild(redTemp);
        this.sobe.forEach(el => {
            document.getElementById("soba" + el).classList.add("crven");
        });
        show(stranica);
    }
}