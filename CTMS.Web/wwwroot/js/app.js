 
// tooltips
document.addEventListener("DOMContentLoaded", function () {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))
});

const currentyearHtml = document.querySelector("#currentyear");
const currentYear = new Date().getFullYear();
currentyearHtml.innerHTML = currentYear;

// Animation buttons
const buttons = document.querySelectorAll('.btn--animation');

for (const button of buttons) {
    for (let i = 0; i < 4; i++) {
        const span = document.createElement('span');
        button.appendChild(span);
    }
}
// Animation buttons end
 