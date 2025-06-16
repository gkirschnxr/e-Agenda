class Calendario {
  constructor() {
    this.currentDate = new Date();
    this.monthYear = document.getElementById("monthYear");
    this.daysContainer = document.getElementById("daysContainer");
    this.monthNames = [
      "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
      "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    ];
  }

  render() {
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();

    const firstDay = new Date(year, month, 1).getDay();
    const totalDays = new Date(year, month + 1, 0).getDate();

    this.monthYear.innerText = `${this.monthNames[month]} ${year}`;
    this.daysContainer.innerHTML = "";

    for (let i = 0; i < firstDay; i++) {
      this.daysContainer.innerHTML += `<div class="day" style="border: none;"></div>`;
    }

    for (let day = 1; day <= totalDays; day++) {
      this.daysContainer.innerHTML += `<div class="day" onclick="alert('Dia ${day}')">${day}</div>`;
    }
  }

  changeMonth(offset) {
    this.currentDate.setMonth(this.currentDate.getMonth() + offset);
    this.render();
  }
}