import { show, fec } from "./include.js";
import { fetchBolnice } from "./index.js";
export class Bolnica {
    constructor(id, ime, lekari, brojMesta, sobe) {
        this.id = id;
        this.ime = ime;
        this.lekari = lekari;
        this.brojMesta = brojMesta;
        this.sobe = sobe;
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
    }
    dajZaposljene(element) {
        fec("/Lekar/DajZaposljeneLekare/" + this.id, "GET", lekari => {
            element.replaceChildren(
                document.getElementById("bolnicaTabelaNaslov"));
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
                dugme.addEventListener("click", () => fec("/Bolnica/OtpustiLekara/" + this.id + "/" + lekar.id, "PUT", () => {
                    this.dajZaposljene(element);
                    this.dajNezaposlene(document.getElementById("bolnicaZaposliIme"));
                }));
                td.appendChild(dugme);
                tr.appendChild(td);

                element.appendChild(tr);
            });
        });
    }
    draw() {
        let stranica = document.getElementById("bolnicaStranica");
        const tabela = document.getElementById("bolnicaTabela");
        const sobe = document.getElementById("bolnicaSobe");
        const bolovanja = document.getElementById("bolnicaBolovanja");
        const naslovBolovanja = document.createElement("h2");
        naslovBolovanja.innerHTML = "Bolovanja u sobi ";
        sobe.replaceChildren();
        let h2 = stranica.querySelector(".donji h2");
        h2.innerHTML = "Bolnica: " + this.ime;

        let h3 = stranica.querySelector("h3");
        h3.innerHTML = "Broj mesta: " + this.brojMesta;

        document.getElementById("bolnicaObrisi").onclick = () => {
            fec("/Bolnica/ObrisiBolnicu/" + this.id, "DELETE",() => {
                fetchBolnice();
            });
        };

        document.getElementById("bolnicaPreimenuj").addEventListener("click", () => {
            let ime = document.getElementById("bolnicaIme").value;
            if(ime.length<30 && ime.length>0)
            fec("/Bolnica/PreimenujBolnicu/" + this.id + "/" + ime,"PUT",() => {
                this.ime = ime;
                document.getElementById("meni" + this.id).innerHTML = this.ime;
                h2.innerHTML = this.ime;
            });
            else alert("Neispravno ime.")
        });
        const sel = document.getElementById("bolnicaZaposliIme");
        this.dajNezaposlene(sel);
        document.getElementById("bolnicaZaposli").onclick = () => {
            if(sel.value)
            fec("/Bolnica/ZaposliLekara/" + this.id + "/" + sel.value,"POST",()=>this.draw());
            else alert("Neispravan lekar.")
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
                document.querySelectorAll(".zelen").forEach(el => el.classList.remove("zelen"));
                document.querySelector(".donji.nestani.bolovanja").style.visibility = "visible";
                dugmeTemp.classList.add("zelen");
                const idSobe = Number(dugmeTemp.id.substring(4));
                fec("/Lecenje/DajBolesnike/" + this.id + "/" + idSobe, "GET", (bolest) => {
                    const pomNaslov = naslovBolovanja.cloneNode(true);
                    pomNaslov.innerHTML = pomNaslov.innerHTML + idSobe;
                    bolovanja.replaceChildren(pomNaslov);
                    if (bolest.length) {
                        const boles = bolest[0];
                        let p = document.createElement("p");
                        p.innerHTML = "<b>Pacijent: </b>" + boles.pacijent.ime + " " + boles.pacijent.prezime +", "+ boles.pacijent.jmbg;
                        bolovanja.appendChild(p);
                        
                        p = document.createElement("p");
                        p.innerHTML = "<b>Lekar: </b>" + boles.lekar.ime + " " + boles.lekar.prezime;
                        bolovanja.appendChild(p);

                        p = document.createElement("p");
                        p.innerHTML = "<b>Početak bolovanja: </b>" + new Date(boles.pocetak).toLocaleDateString();
                        bolovanja.appendChild(p);
                        
                        const dugmeZatvori = document.createElement("button");
                        dugmeZatvori.innerHTML = "Zatvori bolovanje";
                        dugmeZatvori.addEventListener("click", () => {
                            fec("/Lecenje/ZatvoriBolovanje/" + boles.id, "PUT", () => {
                                const q = this.sobe.indexOf(idSobe);
                                if (q > -1)
                                    this.sobe.splice(q, 1);
                                this.draw();
                                const pom = document.getElementById("bolnica"+this.id).children[2].innerHTML;
                                document.getElementById("bolnica"+this.id).children[2].innerHTML = Number(pom)-1;
                            })
                        });
                        bolovanja.appendChild(dugmeZatvori);
                    } else {
                        let divPom = document.createElement("div");
                        const labela = document.createElement("label");
                        const selekt = document.createElement("select");
                        selekt.name = "lekarOdaberi";
                        selekt.id = "lekarOdaberi"
                        labela.for = "lekarOdaberi";
                        labela.innerHTML = "<b>Zeljeni lekar: </b>"
                        this.lekari.forEach(lekar => {
                            const option = document.createElement("option");
                            option.value = lekar.id;
                            option.innerHTML = lekar.ime + " " + lekar.prezime;
                            option.selected = true;
                            selekt.appendChild(option);
                        });

                        divPom.appendChild(labela);
                        divPom.appendChild(selekt);
                        bolovanja.appendChild(divPom);
                        divPom = document.createElement("div");
                        divPom.classList.add("horDiv");
                        const labelaPacijent = document.createElement("label");
                        const selektPacijent = document.createElement("select");
                        selektPacijent.name = "pacijenOdaberi";
                        selektPacijent.id = "pacijenOdaberi"
                        labelaPacijent.for = "pacijentOdaberi";
                        labelaPacijent.innerHTML = "<b>Pacijent: </b>"


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
                            if(!selekt.value) {
                                alert("Neispravan lekar");
                                console.log("AGAAGd11s")
                                return;
                            }
                            if(!selektPacijent.value) {
                                alert("Neispravan pacijent");
                                console.log("AGAAGd")
                                return;
                            }
                            fec("/Lecenje/DodajBolovanje/" + selektPacijent.value + "/" + this.id + "/" + selekt.value + "/" + idSobe, "POST", () => {
                                this.sobe.push(idSobe);
                                this.draw();
                                const pom = document.getElementById("bolnica"+this.id).children[2].innerHTML;
                                document.getElementById("bolnica"+this.id).children[2].innerHTML = Number(pom)+1;
                            })

                        });
                        divPom.appendChild(labelaPacijent);
                        divPom.appendChild(selektPacijent);
                        bolovanja.appendChild(divPom);
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
        document.querySelectorAll(".zelen").forEach(el => el.classList.remove("zelen"));
        document.querySelectorAll(".donji.nestani.lekari").forEach(el => el.style.visibility="visible");
        document.querySelectorAll(".gornji.nestani.sobe").forEach(el => el.style.visibility="visible");
    }
    drawTabela(element) {
        const tr = document.createElement("tr");
        tr.id = "bolnica" + this.id;
        tr.addEventListener("click", () => this.draw());

        let td = document.createElement("td");
        td.innerHTML = this.ime;
        tr.appendChild(td);

        td = document.createElement("td");
        td.innerHTML = this.brojMesta;
        tr.appendChild(td);

        td = document.createElement("td");
        td.innerHTML = this.sobe.length;
        tr.appendChild(td);

        element.appendChild(tr);
    }
}