import { Bolnica } from "./bolnica.js";
import { Pacijent } from "./pacijent.js";
import { Lekar } from "./lekar.js";
import { adr, hideAll, err, show } from "./include.js";
const pacijentTab = document.getElementById("pacijentiTab");
const lekariTab = document.getElementById("lekariTab");
const pacijentiSadrzaj = document.getElementById("pacijentiStranica");
const lekariSadrzaj = document.getElementById("lekariStranica");

let bolnice = [];
const fetchBolnice = () => {
    fetch(adr + "/Bolnica/DajBolnice").then(res => {
        res.json().then(rres => {
            const meni = document.getElementById("bolnicaMeni");
            meni.replaceChildren();
            rres.forEach(bolnica => {
                let bol = new Bolnica(bolnica.id, bolnica.ime, bolnica.lekari, bolnica.brojMesta, bolnica.zauzeteSobe);
                bolnice.push()
                let div = document.createElement("p");
                div.id = "meni" + bolnica.id;
                div.innerHTML = bolnica.ime;
                div.onclick = () => bol.draw();
                meni.appendChild(div);
            });
        }, err);
    }, err);
};
const showLekari = () => {
    hideAll();
    fetchLekari();
    lekariSadrzaj.style.display = "block";
};
const showPacijenti = () => {
    hideAll();
    fetchPacijenti();
    pacijentiSadrzaj.style.display = "block";
};
const fetchLekari = () => {
    fetch(adr + "/Lekar/DajLekare").then(res => {
        res.json().then(rres => {
            const tabela = document.getElementById("lekari");
            const naslov = document.getElementById("lekariNaslov");
            tabela.replaceChildren(naslov);
            const tabelaBol = document.getElementById("lekariBolnice")
            const naslovBol = document.getElementById("lekariBolniceNaslov");
            tabelaBol.replaceChildren(naslov);
            rres.forEach(lekar => {
                let pac = new Lekar(lekar.id, lekar.ime, lekar.prezime, lekar.bolnice, lekar.pacijenti);
                pac.draw(tabela);
            });
        }, err);
    }, err);
};
const fetchPacijenti = () => {
    fetch(adr + "/Pacijent/DajPacijente").then(res => {
        res.json().then(rres => {
            const tabela = document.getElementById("pacijenti");
            const naslov = document.getElementById("pacijentiNaslov");
            tabela.replaceChildren(naslov);
            rres.forEach(pacijent => {
                let pac = new Pacijent(pacijent.id, pacijent.jmbg, pacijent.ime, pacijent.prezime);
                pac.draw(tabela);
            });
        }, err);
    }, err);

};
const dodajLekara = () => {
    const ime = document.getElementById("lekarIme").value;
    const prezime = document.getElementById("lekarPrezime").value;
    fetch(adr + "/Lekar/DodajLekara/" + ime + "/" + prezime, { method: "POST" }).then(res => {
        fetchLekari();
    }, err);
};
const dodajPacijenta = () => {
    const jmbg = document.getElementById("pacijentJmbg").value;
    const ime = document.getElementById("pacijentIme").value;
    const prezime = document.getElementById("pacijentPrezime").value;
    fetch(adr + "/Pacijent/DodajPacijenta/" + ime + "/" + prezime + "/" + jmbg, { method: "POST" }).then(res => {
        fetchPacijenti();
    }, err);
};
const dodajBolnicu = () => {
    const ime = document.getElementById("bolnicaImeDodaj").value;
    const broj = document.getElementById("bolnicaBrojDodaj").value;
    fetch(adr + "/Bolnica/DodajBolnicu/" + ime + "/" + broj, { method: "POST" }).then(res => {
        fetchBolnice();
        alert("Uspesno dodata bolnica");
    }, err);
};
window.addEventListener("load", fetchBolnice);
//window.addEventListener("load", fetchLekari);
//window.addEventListener("load", fetchPacijenti);
window.addEventListener("load", hideAll);
lekariTab.addEventListener("click", showLekari);
pacijentTab.addEventListener("click", showPacijenti);
document.getElementById("pacijentDodaj").addEventListener("click", dodajPacijenta);
document.getElementById("lekarDodaj").addEventListener("click", dodajLekara);
document.getElementById("dodajBolnicuTab").addEventListener("click", () => show(document.getElementById("dodajBolnicuStranica")));
document.getElementById("bolnicaDodaj").addEventListener("click", dodajBolnicu);