

const vacancies = document.querySelectorAll('[data-qa="vacancy-serp__vacancy"]');
const refers = [];
var temp;
var refer;

var q = 0;
for (let i = 0; i < vacancies.length; i++) {

    let vacancy = vacancies[i];

    // URL
    const urlElement = vacancy.querySelector('[data-qa="serp-item__title"]');
    // console.log(urlElement);
    const contactElement = vacancy.querySelector('[data-qa="vacancy-serp__vacancy_contacts"]');
    // console.log(contactElement);
    if (contactElement) {
        console.log(q++ + "=" + i);
        temp = urlElement.getAttribute('href');
        if (temp.includes('?b=')) {
            temp = new URL(temp).searchParams.get('b');
            refer = 'https://adsrv.hh.ru/click?b=' + temp;
        } else {
            refer = temp.split('?')[0];
        }
        refers.push(refer);
        console.log(refer);
    }

}

JSON.stringify(refers);