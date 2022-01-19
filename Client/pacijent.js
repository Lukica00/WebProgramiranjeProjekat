import { Lecenje } from "./lecenje.js";
import { err, drawTabela } from "./include.js";
export class Pacijent {

    constructor(id, jmbg, ime, prezime) {
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.jmbg = jmbg;
    }
    draw(element, imeSeme) {
        const polje = drawTabela(imeSeme, 3, this);
        polje.firstElementChild.id = "pacijent" + this.id;
        polje.firstElementChild.onclick = () => {
            fetch("https://localhost:5001/Lecenje/DajBolovanja/" + this.id).then(res => {
                res.json().then(rres => {
                    console.log(rres);
                    const tabelaBol = document.getElementById("bolovanja")
                    const naslov = document.getElementById("bolovanjaNaslov");
                    tabelaBol.replaceChildren(naslov);
                    rres.forEach(lecenje => {
                        let lec = new Lecenje(lecenje.id, lecenje.pocetak, lecenje.kraj, lecenje.bolnica, lecenje.lekar, lecenje.soba);
                        lec.draw(tabelaBol);
                    });
                }, err);
            }, err);
        };
        const button = polje.querySelector("button");
        button.onclick = () => {
            fetch("https://localhost:5001/Pacijent/ObrisiPacijenta/" + this.id, { method: "DELETE" }).then(res => {
                document.getElementById("pacijent" + this.id).remove();
            }, err);
        };
        element.appendChild(polje);
    }
}