const data = {
    Phone: '',
    Title: '',
    Salary: '',
    Experience: '',
    Compensation: '',
    ScheduleRemote: '',
    Employer: '',
    LocationCity: '',
    LocationMetro: ''
};

var elementContact = document.getElementsByClassName("vacancy-action")[1].getElementsByTagName("button")[0];

function main() {
    // Сначала заполняем все данные, которые можно получить сразу
    //elementContact.click();
    fillOtherData();
    //elementContact.click();
    //var drop = document.querySelector('[data-qa="drop-base"]');
    //if (drop) {
    //    var phoneElement = document.querySelector('[data-qa="vacancy-contacts__phone-number"]');
    //    if (phoneElement) data.Phone = phoneElement.innerText;
    //}
    // Возвращаем результат сразу
    return JSON.stringify(data);
}

function fillOtherData() {
    // Title
    const titleElement = document.querySelector('h1');
    if (titleElement) data.Title = titleElement.textContent.trim();

    // Зарплата
    const salaryElement = document.querySelector('[data-qa="vacancy-salary"]');
    if (salaryElement) data.Salary = salaryElement.textContent.trim();

    // Experience
    const experienceElements = document.querySelector('[data-qa="work-experience-text"]');
    if (experienceElements) data.Experience = experienceElements.textContent.trim();

    // Compensation
    const compensationElement = document.querySelector('[data-qa="compensation-frequency-text"]');
    if (compensationElement) data.Compensation = compensationElement.textContent.trim();

    // Schedule
    const remoteElement = document.querySelector('[data-qa="work-formats-text"]');
    if (remoteElement) data.ScheduleRemote = remoteElement.textContent.trim();

    // Компания
    const companyElement = document.querySelector('[data-qa="vacancy-company-name"]');
    if (companyElement) data.Employer = companyElement.textContent.trim();

    // Местоположение
    const locationElement = document.querySelector('[data-qa="vacancy-view-raw-address"]');
    if (locationElement) data.LocationCity = locationElement.textContent.trim();

    const metroElements = document.querySelectorAll('[data-qa="address-metro-station-name"]');
    if (metroElements.length > 0) {
        const metroStations = Array.from(metroElements).map(el => el.textContent.trim());
        data.LocationMetro = metroStations.join(', ');
    }
}

// Вызываем main и возвращаем результат
main();