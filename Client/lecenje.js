export class Lecenje {
    constructor(id, pocetak, kraj, bolnica, lekar, soba) {
        this.id = id;
        let pocetakDate = new Date(pocetak)
        this.pocetak = pocetakDate.toLocaleDateString();
        let krajDate = new Date(kraj);
        this.kraj = krajDate.getTime()<new Date(null).getTime()?"/":krajDate.toLocaleDateString();
        this.bolnica = bolnica?bolnica.ime:"Zatvorena bolnica";
        this.lekar = lekar.ime+" "+lekar.prezime;
        this.soba = soba;
    }
    draw(el) {
        let tr = document.createElement("tr");
        tr.id = this.id;
        for (const a in this) {
            if (this[a] != this.id) {
                let td1 = document.createElement("td");
                td1.innerHTML = this[a];
                tr.appendChild(td1);
            }
        }
        el.appendChild(tr);
    }
}