import { Lecenje } from "./lecenje.js";
import { err, drawTabela, fec } from "./include.js";
export class Pacijent {

    constructor(id, jmbg, ime, prezime) {
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.jmbg = jmbg;
    }
    draw(element) {
        const tr = document.createElement("tr");
        tr.id = "pacijent" + this.id;
        tr.addEventListener("click", () => {
            fec("/Lecenje/DajBolovanja/" + this.id, "GET", lecenja => {
                const tabelaBol = document.getElementById("pacijentiBolovanja")
                const naslov = document.getElementById("pacijentiBolovanjaNaslov");
                tabelaBol.replaceChildren(naslov);
                lecenja.forEach(lecenje => {
                    let lec = new Lecenje(lecenje.id, lecenje.pocetak, lecenje.kraj, lecenje.bolnica, lecenje.lekar, lecenje.soba);
                    lec.draw(tabelaBol);
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
        td.innerHTML = this.jmbg;
        tr.appendChild(td);

        td = document.createElement("td");
        const dugme = document.createElement("button");
        dugme.innerHTML = "Obrisi";
        dugme.addEventListener("click", () => fec("/Pacijent/ObrisiPacijenta/" + this.id, "DELETE", () =>
            document.getElementById("pacijent" + this.id).remove())
        );
        td.appendChild(dugme);
        tr.appendChild(td);

        element.appendChild(tr);
    }
}