'use strict';
angular.module("ngLocale", [], ["$provide", function($provide) {
var PLURAL_CATEGORY = {ZERO: "zero", ONE: "one", TWO: "two", FEW: "few", MANY: "many", OTHER: "other"};
<<<<<<< HEAD
$provide.value("$locale", {
  "DATETIME_FORMATS": {
    "AMPMS": [
      "asubuhi",
      "alasiri"
=======
function getDecimals(n) {
  n = n + '';
  var i = n.indexOf('.');
  return (i == -1) ? 0 : n.length - i - 1;
}

function getVF(n, opt_precision) {
  var v = opt_precision;

  if (undefined === v) {
    v = Math.min(getDecimals(n), 3);
  }

  var base = Math.pow(10, v);
  var f = ((n * base) | 0) % base;
  return {v: v, f: f};
}

$provide.value("$locale", {
  "DATETIME_FORMATS": {
    "AMPMS": [
      "AM",
      "PM"
>>>>>>> master
    ],
    "DAY": [
      "Jumapili",
      "Jumatatu",
      "Jumanne",
      "Jumatano",
      "Alhamisi",
      "Ijumaa",
      "Jumamosi"
    ],
    "MONTH": [
      "Januari",
      "Februari",
      "Machi",
      "Aprili",
      "Mei",
      "Juni",
      "Julai",
      "Agosti",
      "Septemba",
      "Oktoba",
      "Novemba",
      "Desemba"
    ],
    "SHORTDAY": [
<<<<<<< HEAD
      "J2",
      "J3",
      "J4",
      "J5",
      "Alh",
      "Ij",
      "J1"
=======
      "Jumapili",
      "Jumatatu",
      "Jumanne",
      "Jumatano",
      "Alhamisi",
      "Ijumaa",
      "Jumamosi"
>>>>>>> master
    ],
    "SHORTMONTH": [
      "Jan",
      "Feb",
      "Mac",
      "Apr",
      "Mei",
      "Jun",
      "Jul",
      "Ago",
      "Sep",
      "Okt",
      "Nov",
      "Des"
    ],
    "fullDate": "EEEE, d MMMM y",
    "longDate": "d MMMM y",
    "medium": "d MMM y h:mm:ss a",
    "mediumDate": "d MMM y",
    "mediumTime": "h:mm:ss a",
<<<<<<< HEAD
    "short": "dd/MM/yyyy h:mm a",
    "shortDate": "dd/MM/yyyy",
    "shortTime": "h:mm a"
  },
  "NUMBER_FORMATS": {
    "CURRENCY_SYM": "TSh",
=======
    "short": "dd/MM/y h:mm a",
    "shortDate": "dd/MM/y",
    "shortTime": "h:mm a"
  },
  "NUMBER_FORMATS": {
    "CURRENCY_SYM": "Ksh",
>>>>>>> master
    "DECIMAL_SEP": ".",
    "GROUP_SEP": ",",
    "PATTERNS": [
      {
        "gSize": 3,
        "lgSize": 3,
<<<<<<< HEAD
        "macFrac": 0,
=======
>>>>>>> master
        "maxFrac": 3,
        "minFrac": 0,
        "minInt": 1,
        "negPre": "-",
        "negSuf": "",
        "posPre": "",
        "posSuf": ""
      },
      {
        "gSize": 3,
        "lgSize": 3,
<<<<<<< HEAD
        "macFrac": 0,
        "maxFrac": 2,
        "minFrac": 2,
        "minInt": 1,
        "negPre": "(\u00a4",
        "negSuf": ")",
=======
        "maxFrac": 2,
        "minFrac": 2,
        "minInt": 1,
        "negPre": "\u00a4-",
        "negSuf": "",
>>>>>>> master
        "posPre": "\u00a4",
        "posSuf": ""
      }
    ]
  },
  "id": "sw-ke",
<<<<<<< HEAD
  "pluralCat": function (n) {  if (n == 1) {   return PLURAL_CATEGORY.ONE;  }  return PLURAL_CATEGORY.OTHER;}
=======
  "pluralCat": function (n, opt_precision) {  var i = n | 0;  var vf = getVF(n, opt_precision);  if (i == 1 && vf.v == 0) {    return PLURAL_CATEGORY.ONE;  }  return PLURAL_CATEGORY.OTHER;}
>>>>>>> master
});
}]);