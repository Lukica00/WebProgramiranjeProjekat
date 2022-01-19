import { Bolnica } from "./bolnica.js";
import { Pacijent } from "./pacijent.js";
import { Lekar } from "./lekar.js";
import {adr,hideAll,err} from "./include.js";
const pacijentTab = document.getElementById("pacijentiTab");
const lekariTab = document.getElementById("lekariTab");
const pacijentiSadrzaj = document.getElementById("pacijentiSadrzaj");
const lekariSadrzaj = document.getElementById("lekariSadrzaj");

let bolnice = [];
const fetchBolnice = () => {
    fetch(adr+"/Bolnica/DajBolnice").then(res => {
        res.json().then(rres => {
            const meni = document.getElementById("meniBolnice");
            meni.replaceChildren();
            rres.forEach(bolnica => {
                let bol = new Bolnica(bolnica.id, bolnica.ime, bolnica.prezime, bolnica.brojMesta);
                bolnice.push()
                let div = document.createElement("p");
                div.id="meni"+bolnica.id;
                div.innerHTML = bolnica.ime;
                div.onclick = () => bol.draw(document.getElementById("bolnicaTabela"),"semaBolnicaLekari");
                meni.appendChild(div);
            });
        }, err);
    }, err);
}
const showLekari = () => {
    hideAll();
    lekariSadrzaj.style.display = "block";
    fetchLekari();
};
const showPacijenti = () => {
    hideAll();
    pacijentiSadrzaj.style.display = "block";
    fetchPacijenti();
};
const fetchLekari = () => {
    fetch(adr+"/Lekar/DajLekare").then(res => {
        res.json().then(rres => {
            const tabela = document.getElementById("lekari");
            const naslov = document.getElementById("lekariNaslov");
            tabela.replaceChildren(naslov);
            rres.forEach(lekar => {
                let pac = new Lekar(lekar.id, lekar.ime, lekar.prezime, lekar.bolnice, lekar.pacijenti);
                pac.draw(tabela,"lekariSema");
            });
        }, err);
    }, err);
};
const fetchPacijenti = () => {
    fetch(adr+"/Pacijent/DajPacijente").then(res => {
        res.json().then(rres => {
            const tabela = document.getElementById("pacijenti");
            const naslov = document.getElementById("pacijentiNaslov");
            tabela.replaceChildren(naslov);
            rres.forEach(pacijent => {
                let pac = new Pacijent(pacijent.id, pacijent.jmbg, pacijent.ime, pacijent.prezime);
                pac.draw(tabela,"pacijentiSema");
            });
        }, err);
    }, err);

};
const dodajLekara = () => {
    const ime = document.getElementById("lekarIme").value;
    const prezime = document.getElementById("lekarPrezime").value;
    fetch(adr+"/Lekar/DodajLekara/" + ime + "/" + prezime, { method: "POST" }).then(res => {
        fetchLekari();
    }, err);
};
const dodajPacijenta = () => {
    const jmbg = document.getElementById("pacijentJmbg").value;
    const ime = document.getElementById("pacijentIme").value;
    const prezime = document.getElementById("pacijentPrezime").value;
    fetch(adr+"/Pacijent/DodajPacijenta/" + ime + "/" + prezime + "/" + jmbg, { method: "POST" }).then(res => {
        fetchPacijenti();
    }, err);
};
const otvoriDodajBolnicu = () => {
    hideAll();
    document.getElementById("dodajBolnicuSadrzaj").style.display = "block";
};
const dodajBolnicu = () => {
    const ime = document.getElementById("bolnicaIme").value;
    const broj = document.getElementById("bolnicaBroj").value;
    fetch(adr+"/Bolnica/DodajBolnicu/" + ime + "/" + broj, { method: "POST" }).then(res => {
        fetchBolnice();
    }, err);
};
window.addEventListener("load", fetchBolnice);
window.addEventListener("load", fetchLekari);
window.addEventListener("load", fetchPacijenti);
window.addEventListener("load", hideAll);
lekariTab.addEventListener("click", showLekari);
pacijentTab.addEventListener("click", showPacijenti);
document.getElementById("dodajPacijenta").addEventListener("click", dodajPacijenta);
document.getElementById("dodajLekara").addEventListener("click", dodajLekara);
document.getElementById("otvoriDodajBolnicu").addEventListener("click", otvoriDodajBolnicu);
document.getElementById("dodajBolnicu").addEventListener("click", dodajBolnicu);