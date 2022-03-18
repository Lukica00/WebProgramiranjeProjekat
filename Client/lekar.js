import { fec } from "./include.js";
export class Lekar {
    constructor(id, ime, prezime, bolnice, pacijenti) {
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.bolnice = bolnice;
        this.pacijenti = pacijenti;
    }
    draw(element) {
        const tr = document.createElement("tr");
        tr.addEventListener("click", () => {
            fec("/Bolnica/DajLekaruBolnice/" + this.id, "GET", bolnice => {
                const tabelaBol = document.getElementById("lekariBolnice")
                const naslovBol = document.getElementById("lekariBolniceNaslov");
                tabelaBol.replaceChildren(naslovBol);
                bolnice.forEach(bolnica => {
                    let tr = document.createElement("tr");
                    let td1 = document.createElement("td");
                    td1.innerHTML = bolnica.ime;
                    tr.appendChild(td1);
                    
                    td1 = document.createElement("td");
                    td1.innerHTML = bolnica.brojSoba;
                    tr.appendChild(td1);

                    tr.onclick = () => {
                        document.getElementById("bolnica" + bolnica.id).click();
                    };
                    tabelaBol.appendChild(tr);
                });
            })
        });

        let td = document.createElement("td");
        td.innerHTML = this.ime;
        tr.appendChild(td);

        td = document.createElement("td");
        td.innerHTML = this.prezime;
        tr.appendChild(td);

        td = document.createElement("td");
        td.innerHTML = this.bolnice.length;
        tr.appendChild(td);

        td = document.createElement("td");
        td.innerHTML = this.pacijenti.length;
        tr.appendChild(td);

        element.appendChild(tr);

    }
}