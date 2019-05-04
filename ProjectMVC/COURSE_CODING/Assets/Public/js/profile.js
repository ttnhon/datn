document.getElementById('pf-avatar').addEventListener('click',
    function () {
        document.querySelector('#img-popup').style.height = '100%';
    });

document.querySelector('#img-close').addEventListener('click',
    function () {
        document.querySelector('#img-popup').style.height = '0';
    });



document.getElementById('intro-edit').addEventListener('click',
    function () {
        document.querySelector('#intro-popup').style.height = '100%';
    });

document.querySelector('#intro-close').addEventListener('click',
    function () {
        document.querySelector('#intro-popup').style.height = '0';
    });

document.getElementById('about-edit').addEventListener('click',
    function () {
        document.querySelector('#about-popup').style.height = '100%';
    });

document.querySelector('#about-close').addEventListener('click',
    function () {
        document.querySelector('#about-popup').style.height = '0';
    });

document.querySelector('#defaultCheck').addEventListener('change',
    function () {
        if (this.checked) {
            document.querySelector('#school-end-date').style.visibility = 'hidden';
        } else {
            document.querySelector('#school-end-date').style.visibility = 'visible';
        }
    });

document.getElementById('school-add').addEventListener('click',
    function () {
        document.querySelector('#school-popup').style.height = '100%';
    });

document.querySelector('#school-close').addEventListener('click',
    function () {
        document.querySelector('#school-popup').style.height = '0';
    });