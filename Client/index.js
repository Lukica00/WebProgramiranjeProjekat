import { Bolnica } from "./bolnica.js";
import { Pacijent } from "./pacijent.js";
import { Lekar } from "./lekar.js";
import { fec, show } from "./include.js";
const pacijentTab = document.getElementById("pacijentiTab");
const lekariTab = document.getElementById("lekariTab");
const bolnicaTab = document.getElementById("bolnicaTab");
const pacijentiSadrzaj = document.getElementById("pacijentiStranica");
const lekariSadrzaj = document.getElementById("lekariStranica");
const bolnicaSadrzaj = document.getElementById("bolnicaStranica");

const showLekari = () => {
    fetchLekari();
    show(lekariSadrzaj);
};
const showPacijenti = () => {
    fetchPacijenti();
    show(pacijentiSadrzaj);
};
const showBolnice = () => {
    fetchBolnice();
    show(bolnicaSadrzaj);
};
const fetchLekari = () => {
    fec("/Lekar/DajLekare", "GET", lekari => {
        const tabela = document.getElementById("lekari");
        const naslov = document.getElementById("lekariNaslov");
        tabela.replaceChildren(naslov);
        const tabelaBol = document.getElementById("lekariBolnice")
        const naslovBol = document.getElementById("lekariBolniceNaslov");
        tabelaBol.replaceChildren(naslovBol);
        lekari.forEach(lekar => {
            let pac = new Lekar(lekar.id, lekar.ime, lekar.prezime, lekar.bolnice, lekar.pacijenti);
            pac.draw(tabela);
        });
    });
};
const fetchPacijenti = () => {
    fec("/Pacijent/DajPacijente", "GET", pacijenti => {
        const tabela = document.getElementById("pacijenti");
        const naslov = document.getElementById("pacijentiNaslov");
        tabela.replaceChildren(naslov);
        const pacijentiBolovanja = document.getElementById("pacijentiBolovanja");
        const pacijentiBolovanjaNaslov = document.getElementById("pacijentiBolovanjaNaslov");
        pacijentiBolovanja.replaceChildren(pacijentiBolovanjaNaslov);
        pacijenti.forEach(pacijent => {
            let pac = new Pacijent(pacijent.id, pacijent.jmbg, pacijent.ime, pacijent.prezime);
            pac.draw(tabela);
        });
    });
};

export const fetchBolnice = () => {
    fec("/Bolnica/DajBolnice", "GET", bolnice => {
        const tabela = document.getElementById("bolnicaTabelaBolnice");
        const naslov = document.getElementById("bolnicaTabelaBolniceNaslov");
        tabela.replaceChildren(naslov);
        
        const selekt = document.getElementById("bolnicaZaposliIme");
        selekt.replaceChildren();

        const bolnicaTabela = document.getElementById("bolnicaTabela");
        const bolnicaTabelaNaslov = document.getElementById("bolnicaTabelaNaslov");
        bolnicaTabela.replaceChildren(bolnicaTabelaNaslov);

        bolnice.forEach(bolnica => {
            let bol = new Bolnica(bolnica.id, bolnica.ime, bolnica.lekari, bolnica.brojMesta, bolnica.zauzeteSobe);
            bol.drawTabela(tabela);
        });
        show(bolnicaSadrzaj);
    });
}
const dodajLekara = () => {
    const ime = document.getElementById("lekarIme").value;
    const prezime = document.getElementById("lekarPrezime").value;
    if(ime.length>20 || ime.length<3 || prezime.length>20 || prezime.length<3  || !ime.match(/^\p{L}+$/gu) || !prezime.match(/^\p{L}+$/gu)){
        alert("Neispravno ime i/ili prezime.");
        return;
    }
    fec("/Lekar/DodajLekara/" + ime + "/" + prezime,"POST",() => {
        fetchLekari();
    });
};
const dodajPacijenta = () => {
    const jmbg = document.getElementById("pacijentJmbg").value;
    const ime = document.getElementById("pacijentIme").value;
    const prezime = document.getElementById("pacijentPrezime").value; 
    if(ime.length>20 || ime.length<3 || prezime.length>20 || prezime.length<3 || !ime.match(/^\p{L}+$/gu) || !prezime.match(/^\p{L}+$/gu)){
        alert("Neispravno ime i/ili prezime.");
        return;
    }  
    if(jmbg.length!=13){
        alert("Neispravan JMBG.");
        return;
    }
    if(!jmbg.match(/^[0-9]+$/g)){
        alert("JMBG sme da sadrzi samo brojeve.");
        return;
    }
    fec("/Pacijent/DodajPacijenta/" + ime + "/" + prezime + "/" + jmbg,"POST",() => {
        fetchPacijenti();
    });
};
const dodajBolnicu = () => {
    const ime = document.getElementById("bolnicaImeDodaj").value;
    const broj = document.getElementById("bolnicaBrojDodaj").value;
    
    if(ime.length>30){
        alert("Neispravno ime.");
        return;
    }  
    if(broj<6||broj>20){
        alert("Neispravan broj soba.");
        return;
    }    
    fec("/Bolnica/DodajBolnicu/" + ime + "/" + broj,"POST",() => {
        fetchBolnice();
    });
};
window.addEventListener("load", showBolnice);

lekariTab.addEventListener("click", showLekari);
pacijentTab.addEventListener("click", showPacijenti);
bolnicaTab.addEventListener("click", showBolnice);

document.getElementById("pacijentDodaj").addEventListener("click", dodajPacijenta);
document.getElementById("lekarDodaj").addEventListener("click", dodajLekara);
document.getElementById("bolnicaDodaj").addEventListener("click", dodajBolnicu);
