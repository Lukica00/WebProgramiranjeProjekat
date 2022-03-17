export const err = (error) => {
    console.log("err: " + error);
};
export const hideAll = () => {
    const divovi = document.querySelectorAll(".stranica");
    divovi.forEach(el => {
        el.style.display = "none";
    });
};
export const show = (element) => {
    hideAll();
    element.style.display = "block";
}
export const fec = (adresa, metod, fja) => {
    fetch(adr + adresa, { method: metod }).then(res => {
        if (res.ok)
            if (metod == "GET")
                res.json().then(fja, err);
            else fja();
        else res.text().then(tekst => alert("Greska prilikom feca: " + tekst));
    }, err);
}
export const adr = "https://localhost:5001";

export const drawTabela=(imeSeme,brAtributa,object) =>{
    const sema = document.getElementById(imeSeme);
    const tr = sema.content.cloneNode(true);
    const td = tr.querySelectorAll("td");
    const prop = Object.values(object);
    for (let i = 0; i < brAtributa; i++) {
        td[i].innerHTML = prop[i+1];
    }
    return tr;
};