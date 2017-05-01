$("#query").typeahead({
  source: function (query, process) {
    var strains = [];
    var parameters = { query: query };

    map = {};

    return $.post('/Home/StrainAutoComplete', parameters, function (response) {
      $.each(response, function (i, strain) {
        map[strain.Name] = strain;
        strains.push(strain.Name);
      });

      process(strains);
    });
  },
  updater: function (item) {
    return item;
  }
});