class Mask {

    render() {
        document.getElementById('telefone').addEventListener('input', function (e) {
            let x = e.target.value.replace(/\D/g, '').substring(0, 11);
            let formatted = '';

            if (x.length > 0) formatted += '(' + x.substring(0, 2) + ') ';
            if (x.length > 2) formatted += x.substring(2, 3) + ' ';
            if (x.length > 3) formatted += x.substring(3, 7) + '-';
            if (x.length > 7) formatted += x.substring(7, 11);

            e.target.value = formatted;
        } 
    }
}