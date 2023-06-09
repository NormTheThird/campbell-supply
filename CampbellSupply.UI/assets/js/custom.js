function printDiv(divName) {
    var originalContents = document.body.innerHTML;
    var printButton = document.getElementById("btnPrint");
    printButton.style.visibility = 'hidden';

    var printContents = document.getElementById(divName).innerHTML;
    document.body.innerHTML = printContents;

    window.print();
    printButton.style.visibility = 'visible';
    document.body.innerHTML = originalContents;
    
}