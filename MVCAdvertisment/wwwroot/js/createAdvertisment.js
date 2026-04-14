function showSubscriberForm() {
    document.getElementById("subscriberForm").style.display = "block";
    document.getElementById("companyForm").style.display = "none";
    document.getElementById("adForm").style.display = "none";
    document.getElementById("adPrice").value = "0";
}

function showCompanyForm() {
    document.getElementById("subscriberForm").style.display = "none";
    document.getElementById("companyForm").style.display = "block";
    document.getElementById("adForm").style.display = "none";
    document.getElementById("adPrice").value = "40";
}   

