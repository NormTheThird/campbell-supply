function AutoComplete() {
    // Check If Billing Is Same As Shipping
    if ($('input[type=checkbox]').is(':checked')) {
        // Set Billing Address Same As Shipping Address
        $("input[id$='txtBillFname']").val($("input[id$='txtFname']").val());
        $("input[id$='txtBillLname']").val($("input[id$='txtLname']").val());
        $("input[id$='txtBillAddress']").val($("input[id$='txtAddress']").val());
        $("input[id$='txtBillCity']").val($("input[id$='txtCity']").val());
        $('select[id$="ddlBillStates"]').val($('select[id$="ddlStates"]').val());
        $("input[id$='txtBillZip']").val($("input[id$='txtZip']").val());
        $("input[id$='txtBillPhone']").val($("input[id$='txtPhone']").val());
    }
}

function DisableBilling()
{
    // Check If Billing Is Same As Shipping
    if ($("input[id$='chkBillShip']").is(':checked')) {
        // Disable Billing Text Boxes
        $("input[id$='txtBillFname']").attr("readonly", "true");
        $("input[id$='txtBillLname']").attr("readonly", "true");
        $("input[id$='txtBillAddress']").attr("readonly", "true");
        $("input[id$='txtBillCity']").attr("readonly", "true");
        $('select[id$="ddlBillStates"]').attr("disabled", "disabled");
        $("input[id$='txtBillZip']").attr("readonly", "true");
        $("input[id$='txtBillPhone']").attr("readonly", "true");
    }
    else {
        // Enable Billing Text Boxes
        $("input[id$='txtBillFname']").removeAttr("readonly");
        $("input[id$='txtBillLname']").removeAttr("readonly");
        $("input[id$='txtBillAddress']").removeAttr("readonly");
        $("input[id$='txtBillCity']").removeAttr("readonly");
        $('select[id$="ddlBillStates"]').removeAttr("disabled");
        $("input[id$='txtBillZip']").removeAttr("readonly");
        $("input[id$='txtBillPhone']").removeAttr("readonly");
    }

    // Call AutoComplete
    AutoComplete();
}
