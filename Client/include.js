export const err = () => {
    console.log("err");
};
export const hideAll = () => {
    const divovi = document.querySelectorAll(".sadrzaj");
    divovi.forEach(el => {
        el.style.display = "none";
    });
};
export const adr = "https://localhost:5001";

export const drawTabela=(imeSeme,brAtributa,object) =>{
    const sema = document.getElementById(imeSeme)
    const tr = sema.content.cloneNode(true);
    const td = tr.querySelectorAll("td");
    const prop = Object.values(object);
    for (let i = 0; i < brAtributa; i++) {
        td[i].innerHTML = prop[i+1];
    }
    return tr;
};