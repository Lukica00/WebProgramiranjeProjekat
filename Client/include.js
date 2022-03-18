const err = (error) => {
    console.log("err: " + error);
};
const adr = "https://localhost:5001";
const hideAll = () => {
    const divovi = document.querySelectorAll(".stranica");
    divovi.forEach(el => {
        el.style.display = "none";
    });
    const divoviN = document.querySelectorAll(".nestani");
    divoviN.forEach(el => {
        el.style.visibility = "hidden";
    });
};
export const show = (element) => {
    hideAll();
    element.style.display = "flex";
}
export const fec = (adresa, metod, fja) => {
    fetch(adr + adresa, { method: metod }).then(res => {
        if (res.ok)
            if (metod == "GET")
                res.json().then(fja, err);
            else fja();
        else res.text().then(tekst => alert("Greška prilikom feča: " + tekst));
    }, err);
}