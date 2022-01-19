import { adr, hideAll, err, drawTabela } from "./include.js";
export class Bolnica {
    constructor(id, ime, lekari, brojMesta) {
        this.id = id;
        this.ime = ime;
        this.lekari = lekari;
        this.brojMesta = brojMesta;
    }
    dajNezaposlene(sel) {
        fetch(adr + "/Lekar/DajNezaposljeneLekare/" + this.id).then(res => {
            res.json().then(ress => {
                const sema = document.getElementById("semaBolnica");
                sel.replaceChildren();
                ress.forEach(el => {
                    const p = sema.content.cloneNode(true);
                    let d = p.querySelector("option");
                    d.value = el.id;
                    d.innerHTML = el.ime + " " + el.prezime;
                    sel.appendChild(p);
                })
            })
        }, err);
    };
    dajZaposljene(element, imeSeme) {
        fetch(adr + "/Lekar/DajZaposljeneLekare/" + this.id).then(res => {
            res.json().then(rres => {
                element.replaceChildren();
                rres.forEach(el => {
                    const polje = drawTabela(imeSeme, 3, el);
                    polje.querySelector("button").onclick = () => {
                        fetch(adr + "/Bolnica/OtpustiLekara/"+this.id +"/" + el.id,{method:"PUT"}).then(()=>{
                            this.dajZaposljene(element,imeSeme);
                            this.dajNezaposlene(document.getElementById("zaposliBolnica"));
                        });
            };
            element.appendChild(polje);
        });
    });
});
    };
draw(element, imeSeme) {
    hideAll();
    let div = document.getElementById("bolnicaSadrzaj");
    div.style.display = "flex";

    let h2 = div.querySelector("h2");
    h2.innerHTML = this.ime;

    let h3 = div.querySelector("h3");
    h3.innerHTML = "Broj mesta: " + this.brojMesta;

    document.getElementById("dugmePreimenujBolnicu").onclick = () => {
        let ime = document.getElementById("imeBolnica").value;
        fetch(adr + "/Bolnica/PreimenujBolnicu/" + this.id + "/" + ime, { method: "PUT" }).then(() => {
            this.ime = ime;
            document.getElementById("meni" + this.id).innerHTML = this.ime;
            h2.innerHTML = this.ime;
        }, err);
    }
    document.getElementById("dugmeObrisiBolnicu").onclick = () => {
        fetch(adr + "/Bolnica/ObrisiBolnicu/" + this.id, { method: "DELETE" }).then(() => {
            document.getElementById("meni" + this.id).remove();
            hideAll();
        }, err);
    }
    const sel = document.getElementById("zaposliBolnica");
    this.dajNezaposlene(sel);
    document.getElementById("dugmeZaposli").onclick = () => {
        fetch(adr + "/Bolnica/ZaposliLekara/" + this.id + "/" + sel.value, { method: "POST" }).then(() => {
            this.dajNezaposlene(sel);
            this.dajZaposljene(element, imeSeme);
        });
    };
    this.dajZaposljene(element, imeSeme);
}
}