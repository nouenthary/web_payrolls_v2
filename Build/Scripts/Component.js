let columns = [
  {
    title: '#',
    dataIndex: 'no'
  },
  {
    title: 'HOD',
    dataIndex: 'hod'
  },
  {
    title: 'Company',
    dataIndex: 'company',
  },
  {
    title: 'Product Type',
    dataIndex: 'product_type'
  },
  {
    title: 'Type Name',
    dataIndex: 'type_name'
  },
];

let data = [
  {
    no: 1,
    name: "Admin",
    name1: "Admin",
    name2: "Admin",
    name4: "Admin",
  },
  {
    no: 1,
    name: "Admin",
    name1: "Admin",
    name2: "Admin",
    name4: "Admin",
  },
  {
    no: 1,
    name: "Admin",
    name1: "Admin",
    name2: "Admin",
    name4: "Admin",
  },
  {
    no: 1,
    name: "Admin",
    name1: "Admin",
    name2: "Admin",
    name4: "Admin",
  }
];


let dataForSent = {
  id: 1,
  __RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value
};
//console.log(dataForSent);


Vue.component('table-data',{
  data: function (){
    return {
      columns: columns,
      data: data
    }
  },
  template: '<table class="table bg-white">' +
      '<thead>' +
      '<tr>'+
      '<th v-for="item in columns">{{item.title}}</th>' +
      '</tr>'+
      '</thead>' +
      '<tbody>'+
      '<tr v-for="item in data">' +
      '<td>{{item.no}}</td>' +
      '<td>{{item.name}}</td>' +
      '<td>{{item.name1}}</td>' +
      '<td>{{item.name2}}</td>' +
      '<td>{{item.name4}}</td>' +
      '</tr>'+
      '</tbody>'+
      '</table>',
});


Vue.component('select-option',{
  data: function (){
    return {
      option : [10, 20, 30, 40, 50, 100, "All"]
    }
  },
  template: "<div class='row'> <span>Show : </span> <select class='custom-select-form' id='pageSize' name='pageSize'>" +
      "<option v-for='item in option'>{{item}}</option>" +
      "</select> " + " <span>Entries</span> </div>"
});

const app = new Vue({
  el: '#vueApp',
  data: {
    columns: columns,
    data: "Hello Word"
  }
});