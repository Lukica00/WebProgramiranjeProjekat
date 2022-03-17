import { drawTabela, err, fec } from "./include.js";
export class Lekar {
    constructor(id, ime, prezime, bolnice, pacijenti) {
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.bolnice = bolnice;
        this.pacijenti = pacijenti;
        //this.draw = (element, imeSeme) => element.appendChild(drawTabela(imeSeme, 4, this));
    }
    draw(element) {
        const tr = document.createElement("tr");
        //tr.id = "lekar" + this.id;
        tr.addEventListener("click", () => {
            fec("/Bolnica/DajLekaruBolnice/" + this.id, "GET", bolnice => {
                const tabelaBol = document.getElementById("lekariBolnice")
                const naslov = document.getElementById("lekariBolniceNaslov");
                tabelaBol.replaceChildren(naslov);
                bolnice.forEach(bolnica => {
                    let tr = document.createElement("tr");
                    let td1 = document.createElement("td");
                    td1.innerHTML = bolnica.ime;
                    tr.onclick = () => {
                        document.getElementById("meni" + bolnica.id).click();
                    };
                    tr.appendChild(td1);
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