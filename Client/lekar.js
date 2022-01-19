import { drawTabela } from "./include.js";
export class Lekar {
    constructor(id, ime, prezime, bolnice, pacijenti) {
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.bolnice = bolnice;
        this.pacijenti = pacijenti;
        this.draw = (element, imeSeme) => element.appendChild(drawTabela(imeSeme, 4, this));
    }
}