$(function () {
    console.log('ddd')
    let token = $('input[name="__RequestVerificationToken"]').val();
    let bid = $('#txtManager');
    let cid = $('#txtCompany');
    let lid = $('#txtLocation');
    let status = $('#txtStatus');
    let start = $('#txtStartDate');
    let end = $('#txtStartEnd');
   
    function GetSearchObject() {
        return {
            bid: bid.val(),
            cid: cid.val(),
            lid: lid.val(),
            type: status.val(),
            start: start.val(),
            end: end.val()
        }
    }

    // load voucher
    function LoadVoucher() {
        let objSearch = GetSearchObject();
        $('#show-voucher').load('/Voucher/GetTable', {
            ...objSearch,
            pageSize: $('#pageSize').val()
        });
    }

   // LoadVoucher();
    
    // load voucher            
    $('#btn-search-voucher').click(function () {
        alert('')
        LoadVoucher();
    });
    //
    $(document).on('change', '#show-voucher #pageSize', function () {
        let objSearch = GetSearchObject();
        $('#show-voucher').load('/Voucher/GetTable', {
            pageSize: $(this).val(),
            ...objSearch
        });
    });
    // 
    $(document).on('click', '#show-voucher a', function (e) {
        try {
            let link = $(this).attr("href").split('=');
            let objSearch = GetSearchObject();
            $('#show-voucher').load('/Voucher/GetTable', {
                pageSize: $('#pageSize').val(),
                page: link[1],
                ...objSearch
            });
            e.preventDefault();
        } catch (ex) {
        }
    });
    
    // Load Voucher
    
    
    // Load Voucher NO
    // function LoadTableVoucherNo(){
    //     let objSearch = GetSearchObject();
    //     $('#show-voucher').load('/Voucher/GetTable', {
    //         ...objSearch,
    //         pageSize: $('#pageSize').val()
    //     });
    // }
    //
    // LoadTableVoucherNo();
    
    // Load Voucher No
    
    // Export
    $(document).on('click', '#show-voucher #btn-export-voucher', function () {
        $('#table-voucher').csvExport();
    });
   
    // manager
    bid.GetDropListSelect({
        url: "/Other/GetCompany",
        token : token,
        supChild: '#txtCompany',
        removeChild: true,
        child: [{key: "txtLocation", value : "Location"}],
        label: "Company"
    });
    // manager

    // company
    cid.GetDropListSelect({
        url: "/Other/GetLocation",
        token: token,
        supChild : '#txtLocation',
        label: "Location"
    });
    // company

});