import { Bolnica } from "./bolnica.js";

const err = () => {
    console.log("err");
}

let bolnice = [];
const fetchBolnice = ()=>{
    fetch("https://localhost:5001/Bolnica/DajBolnice").then((res) => {
        res.json().then((rres) => {
            rres.forEach(bolnica => {
                bolnice.push(new Bolnica(bolnica.id, bolnica.ime,bolnica.prezime,bolnica.brMesta))
                let div = document.createElement("p");
                div.innerHTML = bolnica.ime;
                document.getElementById("bolnicaTab").appendChild(div);
            });
        }, err);
    }, err);
}
window.addEventListener("load", fetchBolnice);