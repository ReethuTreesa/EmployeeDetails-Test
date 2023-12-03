/******/ (function (modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if (installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
            /******/
        }
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
            /******/
        };
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
        /******/
    }
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function (exports, name, getter) {
/******/ 		if (!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
            /******/
        }
        /******/
    };
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function (exports) {
/******/ 		if (typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
            /******/
        }
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
        /******/
    };
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function (value, mode) {
/******/ 		if (mode & 1) value = __webpack_require__(value);
/******/ 		if (mode & 8) return value;
/******/ 		if ((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if (mode & 2 && typeof value != 'string') for (var key in value) __webpack_require__.d(ns, key, function (key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
        /******/
    };
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function (module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
        /******/
    };
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function (object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
    /******/
})
/************************************************************************/
/******/({

/***/ "./Scripts/calendar.js":
/*!*****************************!*\
  !*** ./Scripts/calendar.js ***!
  \*****************************/
/*! no static exports found */
/***/ (function (module, exports) {

            eval("var currentEvent;\n\n    var formatDate = function formatDate(date) {\n  return date === null ? '' : moment(date).format(\"MM/DD/YYYY h:mm A\");\n};\n\nvar fpStartTime = flatpickr(\"#StartTime\", {\n  enableTime: true,\n minTime: '09:00',\n defaultDate: new Date(),\n maxDate: \"today\",\n dateFormat: \"m/d/Y h:i K\"\n});\nvar fpEndTime = flatpickr(\"#EndTime\", {\n  enableTime: true,\n minTime: '09:00',\n maxDate: \"today\",\n  dateFormat: \"m/d/Y h:i K\"\n});\n$('#calendar').fullCalendar({\n  defaultView: 'agendaWeek',\n editable: true, \n  views: { \n settimana: { \n type: 'basic', \n duration: { \n days: 7 \n }, \n title: 'Apertura', \n  hiddenDays: [0, 6] \n  } \n }, \n   defaultView: 'settimana', \n lang: 'es', \n  defaultDate: $.fullCalendar.moment().startOf('week'), \n  minTime: \"09\" + \":00:00\", \n  maxTime: \"20\" + \":00:00\", \n timeFormat: 'H:mm',\n axisFormat: 'H:mm',\n slotLabelFormat: \"h:mm A\", \n businessHours: { \n dow: [1, 2, 3, 4, 5], \n  start: '08:00', \n  end: '17:00', \n  }, \n  hiddenDays: [0, 6], \n  height: 'parent',\n  timezone: \"local\",\n header: {\n    left: 'prevYear,prev,next,nextYear today',\n    center: 'title',\n    right: 'month,basicWeek,basicDay'\n  },\n  eventRender: function eventRender(event, $el) {\n $('#calendar button.fc-today-button').removeAttr('disabled'); \n $('#calendar button.fc-today-button').removeClass('fc-state-disabled'); \n   $el.qtip({\n      content: {\n        title: '<span class=\"title\">Status: </span>' + event.status,\n        text: '<span class=\"title\">Start: </span>' + ($.fullCalendar.formatDate(event.start, 'hh:mm A')) + '<br><span class=\"title\">End: </span>' + ($.fullCalendar.formatDate(event.end, 'hh:mm A')) + '<br><span class=\"title\">Client: </span>' + event.client_Name + '<br><span class=\"title\">Project: </span>' + event.project_Name + '<br><span class=\"title\">Activity: </span>' + event.activity_Name + '<br><span class=\"title\">Remarks: </span>' + event.remarks \n      },\n   style: { \n width: 300,\n padding: 5,\n color: 'black', \n textAlign: 'left',\n border: { \n width: 1, \n radius: 3 \n }, \n tip: 'topLeft', \n classes: { \n tooltip: 'ui-widget', \n tip: 'ui-widget', \n title: 'ui-widget-header', \n content: 'ui-widget-content' \n } \n }, \n    hide: {\n        event: 'unfocus'\n      },\n      show: {\n        solo: true\n      },\n      position: {\n        my: 'top left',\n        at: 'bottom left',\n        viewport: $('#calendar-wrapper'),\n        adjust: {\n          method: 'shift'\n        }\n      }\n    });\n  },\n  //events: '/Home/GetTimesheetDetails',\n events: function (start, end, timezone, callback) {  $.ajax({ \n  url: '/Home/GetTimesheetDetails', \n type: \"GET\", \n dataType: \"JSON\", \n success: function (result) { \n  var eventsList = []; \n  $(result).each(function () { \n var result = $(this).attr('result'); \n  if (result == \"session expired\") { \n window.location.href = \"Login/Index\"; \n  } \n var Status = $(this).attr('status'); \n var Date = $(this).attr('date');\n var Start = $(this).attr('start');\n var Id = $(this).attr('timeSheetHead_Id');\n  var timeSheetDet_Id = $(this).attr('timeSheetDet_Id');\n var project_Id = $(this).attr('project_Id');\n var client_Id = $(this).attr('client_Id');\n var activity_Id = $(this).attr('activity_Id');\n var client_Name = $(this).attr('client_Name'); \n var project_Name = $(this).attr('project_Name'); \n var activity_Name = $(this).attr('activity_Name'); \n var remarks = $(this).attr('remarks');\n var End = $(this).attr('end');\n eventsList.push( \n { \n timeSheetHead_Id: Id, \n status: Status, \n title: Status, \n date: Date, \n start: Start, \n client_Name: client_Name, \n project_Name: project_Name,\n activity_Name: activity_Name, \n remarks: remarks, \n end: End,\n project_Id: project_Id,\n client_Id: client_Id,\n activity_Id: activity_Id,\n timeSheetDet_Id: timeSheetDet_Id }); \n }); \n if (callback) \n callback(eventsList); \n }, \n error: function (result) { \n // Alert on error  };\n } \n }) \n }, eventClick: updateEvent,\n  selectable: true,\n  select: addEvent\n});\n/**\r\n * Calendar Methods\r\n **/\n\nfunction updateEvent(event, element) {\n if (moment().format('YYYY-MM-DD') === event.start.format('YYYY-MM-DD') || event.start.isBefore(moment())) { \n currentEvent = event;\n  if ($(this).data(\"qtip\")) $(this).qtip(\"hide\");\n  $('#eventModalLabel').html('Edit');\n  $('#eventModalSave').html('Update');\n $('#TimeSheetHead_Id').val(event.timeSheetHead_Id);\n   $('#TimeSheetDet_Id').val(event.timeSheetDet_Id);\n $('#Client_Id').val(event.client_Id);\n    var clientId = event.client_Id;\n  $.getJSON('/Home/GetProjectList', { ClientId: clientId }, function (data) {    var item = '';\n   $('#Project_Id').find('option').not(':first').remove();\n  item += '<option value=\"\">Select Project</option>';\n  $.each(data, function (i, project) {  if (project.value = event.project_Id) { item += '<option selected=\"selected\" value=\"' + project.value + '\">' + project.text + '</option>'   }   else { item += '<option value=\"' + project.value + '\">' + project.text + '</option>' }     });\n  $('#Project_Id').html(item); });\n $('#Project_Id').val(event.project_Id);\n  $('#Activity_Id').val(event.activity_Id);\n  $('#Remarks').val(event.remarks);\n  $('#isNewEvent').val(false);\n  var start = formatDate(event.start);\n  var end = formatDate(event.end);\n  fpStartTime.setDate(start);\n  fpEndTime.setDate(end);\n  $('#StartTime').val(start);\n  $('#EndTime').val(end);\n\n   $('#eventModal').modal('show');\n } }\n\nfunction addEvent(start, end) {\n  if (moment().format('YYYY-MM-DD') === start.format('YYYY-MM-DD') || start.isBefore(moment()))  { \n // This allows today and future date \n $('#eventForm')[0].reset();\n  $('#eventModalLabel').html('Add Event');\n  $('#eventModalSave').html('Create');\n  $('#isNewEvent').val(true);\n var today = moment(); \n  start.set({ hours: today.hours(), minute: today.minutes() });\n  start = formatDate(start);\n  end = formatDate(end);\n  fpStartTime.setDate(start);\n  fpEndTime.setDate(start);\n  $('#StartTime').val(start); \n  $('#EndTime').val(start); \n $('#Project_Id').find('option').not(':first').remove();\n  $('#eventModal').modal('show');\n } else { \n // Else part is for past dates \n } }\n/**\r\n * Modal\r\n * */\n\n\n$('#eventModalSave').click(function () {\n const timeSheetHeadId = $('#TimeSheetHead_Id').val();\n const timeSheetDetId = $('#TimeSheetDet_Id').val();\n  const clientId = $('#Client_Id').val(); \n const clientName = $('#Client_Id option:selected').text(); \n const projectId = $('#Project_Id').val(); \n const projectName = $('#Project_Id option:selected').text(); \n const activityId = $('#Activity_Id').val(); \n const activityName = $('#Activity_Id option:selected').text(); \n const remarks = $('#Remarks').val(); \n  var startTime = moment($('#StartTime').val());\n var fromTime = moment($('#StartTime').val()).format('DD/MM/YYYY h:mm A');\n var date = moment($('#StartTime').val()).format('DD/MM/YYYY');\n var endTime = moment($('#EndTime').val());\n var toTime = moment($('#EndTime').val()).format('DD/MM/YYYY h:mm A');\n var isAllDay = $('#AllDay').is(\":checked\");\n  var isNewEvent = $('#isNewEvent').val() === 'true' ? true : false;\n\n  if (clientId == \"\") { \n Swal.fire('Please select client'); \n  return; \n   }\n else if (projectId == \"\") { \n Swal.fire('Please select project'); \n  return; \n   }\n  else if (activityId == \"\") { \n Swal.fire('Please select activity'); \n  return; \n   }\n else if (startTime > endTime) {\n    Swal.fire('Start Time cannot be greater than End Time');\n    return;\n  } else if(startTime._i == endTime._i)  {\n    Swal.fire('Start Time cannot be equal to End Time');\n    return;\n  } else if ((!startTime.isValid() || !endTime.isValid()) && !isAllDay) {\n    Swal.fire('Please enter both Start Time and End Time');\n    return;\n  } \n  else if (remarks == \"\") { \n Swal.fire('Please enter remarks'); \n  return; \n   }\n  var event = {\n title: 'Draft',\n status: 'Draft',\n  timeSheetHeadId: timeSheetHeadId,\n timeSheetDetId:timeSheetDetId,\n clientId: clientId,\n  projectId: projectId,\n activityId: activityId,\n client_Name: clientName,\n  project_Name: projectName,\n activity_Name: activityName,\n   date: date,\n  remarks: remarks,\n    isAllDay: isAllDay,\n    startTime: startTime._i,\n    endTime: endTime._i,\n  fromTime: fromTime,\n toTime: toTime };\n\n  if (isNewEvent) {\n    sendAddEvent(event);\n  } else {\n    sendUpdateEvent(event);\n  }\n});\n\nfunction sendAddEvent(event) { \n  axios({\n    method: 'post',\n    url: '/Timesheet/Add',\n    data: { \n  \"Client_Id\": event.clientId, \n  \"Project_Id\": event.projectId, \n  \"Activity_Id\": event.activityId, \n  \"Remarks\": event.remarks,\n      \"Start\": event.startTime,\n  \"Date\": event.date,\n     \"End\": event.endTime,\n \"FromTime\": event.fromTime,\n \"ToTime\": event.toTime,\n }\n  }).then(function (res) {\n   var message = res.data.objResultSet.errorMsgVal;\n var errorCode = res.data.objResultSet.errorCodeVal;\n Swal.fire(message);\n $('#Project_Id').find('option').not(':first').remove();\n if(errorCode == '0'){ var newEvent = {\n        start: event.startTime,\n        end: event.endTime,\n  allDay: event.isAllDay,\n  title: event.title,\n  remarks: event.remarks,\n  status: event.status,\n  timesheetId: res.data.objResultSet.pidVal,\n timeSheetHead_Id: res.data.objResultSet.pidVal,\n timeSheetDet_Id: res.data.objResultSet.didVal,\n activity_Name: event.activity_Name,\n activity_Id: event.activityId,\n client_Name: event.client_Name,\n client_Id: event.clientId,\n project_Name: event.project_Name,\n project_Id: event.projectId,\n     };\n      $('#calendar').fullCalendar('renderEvent', newEvent);\n   }   $('#calendar').fullCalendar('unselect');\n      $('#eventModal').modal('hide');\n  }).catch(function (err) {\n    return alert(\"Something went wrong\");\n  });\n}\n\nfunction sendUpdateEvent(event) {\n  axios({\n    method: 'post',\n    url: '/Timesheet/UpdateDetails',\n    data: { \n   \"TimeSheetHead_Id\": event.timeSheetHeadId,\n  \"TimeSheetDet_Id\": event.timeSheetDetId,\n \"Client_Id\": event.clientId, \n  \"Project_Id\": event.projectId, \n  \"Activity_Id\": event.activityId, \n  \"Remarks\": event.remarks,\n      \"Start\": event.startTime,\n  \"Date\": event.date,\n     \"End\": event.endTime,\n \"FromTime\": event.fromTime,\n \"ToTime\": event.toTime,\n }\n  }).then(function (res) {\n    var message = res.data.objResultSet.errorMsgVal;\n var errorCode = res.data.objResultSet.errorCodeVal;\n Swal.fire(message);\n  currentEvent.status = event.status;\n      currentEvent.Remarks = event.Remarks;\n      currentEvent.start = event.startTime;\n      currentEvent.end = event.endTime;\n      currentEvent.allDay = event.isAllDay;\n      $('#calendar').fullCalendar('updateEvent', currentEvent);\n      $('#eventModal').modal('hide');\n  }).catch(function (err) {\n    return alert(\"Something went wrong: \".concat(err));\n  });\n}\n\n$('#deleteEvent').click(function () {\n  if (confirm(\"Do you really want to delete \\\"\".concat(currentEvent.title, \"\\\" item?\"))) {\n    axios({\n      method: 'post',\n      url: '/Timesheet/DeleteDetails',\n      data: {\n        \"TimeSheetHead_Id\": currentEvent.timeSheetHead_Id,\n \"TimeSheetDet_Id\": currentEvent.timeSheetDet_Id,\n \"Date\": currentEvent.date,\n \"FromTime\": currentEvent.start,\n \"ToTime\": currentEvent.end,\n    }\n    }).then(function (res) {\n      var message = res.data.objResultSet.errorMsgVal;\n\n      if (res.data.objResultSet.errorCodeVal === '0') {\n Swal.fire(message);\n   $('#calendar').fullCalendar('removeEvents', currentEvent._id);\n        $('#eventModal').modal('hide');\n      } else {\n        alert(\"Something went wrong: \".concat(message));\n      }\n    }).catch(function (err) {\n      return alert(\"Something went wrong: \".concat(err));\n    });\n  }\n});\n$('#AllDay').on('change', function (e) {\n  if (e.target.checked) {\n    $('#EndTime').val('');\n    fpEndTime.clear();\n    this.checked = true;\n  } else {\n    this.checked = false;\n  }\n});\n$('#EndTime').on('change', function () {\n  //$('#AllDay')[0].checked = false;\n});\n\n//# sourceURL=webpack:///./Scripts/calendar.js?");

            /***/
        }),

/***/ "./Styles/calendar.scss":
/*!******************************!*\
  !*** ./Styles/calendar.scss ***!
  \******************************/
/*! no static exports found */
/***/ (function (module, exports, __webpack_require__) {

            eval("// extracted by mini-css-extract-plugin\n\n//# sourceURL=webpack:///./Styles/calendar.scss?");

            /***/
        }),

/***/ 0:
/*!**********************************************************!*\
  !*** multi ./Scripts/calendar.js ./Styles/calendar.scss ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function (module, exports, __webpack_require__) {

            eval("__webpack_require__(/*! ./Scripts/calendar.js */\"./Scripts/calendar.js\");\nmodule.exports = __webpack_require__(/*! ./Styles/calendar.scss */\"./Styles/calendar.scss\");\n\n\n//# sourceURL=webpack:///multi_./Scripts/calendar.js_./Styles/calendar.scss?");

            /***/
        })

    /******/
});