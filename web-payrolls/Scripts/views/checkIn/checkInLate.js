$(function () {
  //
  SetValueByControl('#BossId', '/App/GetBoss');
  SetValueByControl('#CompanyId', '/App/GetCompany');
  SetValueByControl('#LocationId', '/App/GetLocation');
  SetValueByControl('#DepartmentId', '/App/GetDepartment');
  SetValueByControl('#PositionId', '/App/GetPosition');
  SetValueByControl('#StaffId', '/App/GetStaff');
  SetValueByControl('#Tell', '/App/GetTell');
  SetValueByControl('#IdCard', '/App/GetIdCard');

  function GetObject() {
    let $form = $("#form-search");
    let data = getFormData($form);
    return data;
  }

  // load table
  function Load() {
    let data = GetObject();
    $('#show').load('/CheckIn/GetTable', data);
  }

  Load();

  // form submit
  $(document).on('submit', '#form-search', function (e) {
    e.preventDefault();
    Load();
  });

  // page Size
  $('#pageSize_').change(function (e) {
    e.preventDefault();
    $('#PageSize').val($(this).val());
    $('#form-search').submit();
  });

  $(document).on('click', '#show-page a', function (e) {
    try {
      let link = $(this).attr("href").split('=');
      let data = GetObject();
      $('#show').load('/CheckIn/GetTable', {
        page: link[1],
        ...data
      });
      e.preventDefault()
    } catch (ex) {
    }
  });

  // Export
  $(document).on('click', '#btn-export', function () {
    $('#table-check-in').csvExport();
  });

  //
  Select2('#BossId');
  Select2('#CompanyId');
  Select2('#LocationId');
  Select2('#DepartmentId');
  Select2('#PositionId');
  Select2('#StaffId');
  Select2('#Tell');
  Select2('#IdCard');
  Select2('#Status');

  // Filter

  // BossId
  $('#BossId').change(function () {
    $('select#CompanyId').empty();
    $('select#LocationId').empty();
    $('select#DepartmentId').empty();
    $('select#PositionId').empty();
    $('select#StaffId').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#CompanyId', dataForSent, '/Other/GetCompany');

    Load();
  });

  // Company
  $('#CompanyId').change(function () {
    $('select#LocationId').empty();
    $('select#DepartmentId').empty();
    $('select#PositionId').empty();
    $('select#StaffId').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#LocationId', dataForSent, '/Other/GetLocation');

    Load();

  });

  //location
  $('#LocationId').change(function () {
    $('select#DepartmentId').empty();
    $('select#PositionId').empty();
    $('select#StaffId').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#DepartmentId', dataForSent, '/Other/GetDepartment');

    Load();
  });

  //Department
  $('#DepartmentId').change(function () {

    $('select#PositionId').empty();
    $('select#StaffId').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#PositionId', dataForSent, '/Other/GetPosition');

    Load();
  });

  //Department
  $('#PositionId').change(function () {

    $('select#StaffId').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#StaffId', dataForSent, '/Other/GetStaff');

    Load();
  });

  // Staff
  $('#StaffId').change(function () {
    Load();
  });

  // tell
  $('#Tell').change(function () {
    Load();
  });

  // tell
  $('#IdCard').change(function () {
    Load();
  });

  // tell
  $('#Status').change(function () {
    Load();
  });

  // Start Date
  $('#StartDate').change(function () {
    Load();
  });

  // End date
  $('#EndDate').change(function () {
    Load();
  });

  // Type
  $('#Type').change(function () {
    Load();
  });

  //Filter

});
