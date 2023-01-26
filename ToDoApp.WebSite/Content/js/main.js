var TODOLIST = {};

TODOLIST.Module = (function ($) {

    var _self = this;
    var _options = {
        doneClass: "done",
        type: "group",
        editType: false,
        todoId: 0,
        taskTodoId: 0
    }

    function init() {
        moduleCache();
        getList();
        eventListener();
    }

    function moduleCache() {
        _self.$addToListItemModal = $("#addToListItemModal");
        _self.$groupList = $("#groupList");
        _self.$marksAsDoneGroup = $("#marksAsDoneGroup");
        _self.$addItemGroup = $("#addItemGroup");
        _self.$list = $("#list");
        _self.$marksAsDoneList = $("#marksAsDoneList");
        _self.$addItemList = $("#addItemList");

        _self.$saveBtn = $("#saveBtn");

        _self.$itemName = $("#name");
        _self.$datetime = $("#datetime");
    }

    function getListTemplate(obj) {
        return '<li data-id="' + obj.Id + (obj.ToDoListId != undefined ? '" data-todoid="'+ obj.ToDoListId + '"' : '') + '">' +
          '<input type="checkbox" value="' + obj.Id + '" name="" ' + (obj.IsChecked ? 'checked' : '') + '>' +
          '<span class="text"> ' + obj.Title + '</span>' +
          (obj.NotificationDate != null ? '<small class="label label-default"><i class="fa fa-clock-o"></i> ' + new Date(parseInt(obj.NotificationDate.substr(6))).format("dd.mm.yyyy HH:MM", true) + '</small>' : '') +
          '<div class="tools">' +
          '<i class="fa fa-edit"></i>' +
          '<i class="fa fa-trash-o"></i>' +
          '</div>' +
          '</li>'
    }

    function getList() {
        $.get("/todolist/get", function (data, textStatus) {
            console.log(textStatus)
            var resultHtml = '';
            $.each(data, function (i, e) {
                resultHtml += getListTemplate(e);
            });

            var element = _self.$groupList;
            element.html(resultHtml);
            marksAsDoneProccess("group");
        }, "json");
    }

    function getTasks(id) {
        $.get("/task/get?toDoListId=" + id, function (data, textStatus) {
            console.log(textStatus)
            var resultHtml = '';
            $.each(data, function (i, e) {
                resultHtml += getListTemplate(e);
            });

            var element = _self.$list;
            element.html(resultHtml);
            $('#listBlock').removeClass("hidden");
            marksAsDoneProccess("list");
        }, "json");
    }

    function marksAsDoneProccess(type, e) {
        if (e != undefined) {
            editPostMethod(type, {
                Id: $(e).parent().data("id"),
                Title: $(e).siblings("span").text(),
                IsChecked: $(e).is(":checked")
            });
        }
        if (type == "group") {
            _self.$groupList.find("input").parent().removeClass(_options.doneClass);
            _self.$groupList.find("input:checked").parent().addClass(_options.doneClass);
            //if (_self.$groupList.find("input:checked")[0] != undefined) {
            //    _self.$list.find("input").prop("checked", true).parent().addClass(_options.doneClass);
            //} else {
            //    _self.$list.find("input").prop("checked", false).parent().removeClass(_options.doneClass)
            //}

        } else {
            self.$list.find("input").parent().removeClass(_options.doneClass);
            _self.$list.find("input:checked").parent().addClass(_options.doneClass);
        }
    }

    function deleteProccess($ths) {
        var id = $ths.parent().parent().data("id");
        $ths.parent().parent().remove();

        //Buraya delete ajax method yazılabilir.
        $.post(_options.type == "group" ? '/todolist/delete' : '/task/delete',
            { id : id},
            function (data, status) {
                console.log("success");
                if (_options.type == "group")
                    getList();
                else
                    getTasks(_options.todoId);
                _self.$addToListItemModal.modal("hide");
            }
        );
    }

    function addItemProccess() {
        if (_self.$itemName.val()) {
            var model = {
                Title: _self.$itemName.val(),
                NotificationDate: _self.$datetime.val()
            };

            if (_options.type == "list") {
                model.ToDoListId = _options.todoId;
            }
            addPostMethod(model);
            clearModal();
        }
    }

    function addPostMethod(model) {
        $.post(_options.type == "group" ? '/todolist/post' : '/task/post',
            model,
            function (data, status) {
                console.log("success");
                if (_options.type == "group")
                    getList();
                else
                    getTasks(_options.todoId);
                _self.$addToListItemModal.modal("hide");
            }
        );
    }

    function editProccess(type, option) {
        var dateElement = $(".edit").find(".label");
        var nameElement = $(".edit").find(".text");
        var date = dateElement.text();
        var name = nameElement.text();

        if (type != "post") {
            _self.$itemName.val(name);
            _self.$datetime.val(date);
        } else if (_self.$itemName.val()) {
            nameElement.text(_self.$itemName.val());
            dateElement.text(_self.$datetime.val());

            editPostMethod(option, {
                Id: _options.todoId,
                Title: _self.$itemName.val(),
                NotificationDate: _self.$datetime.val()
            });

            clearModal();
        }
    }

    function editPostMethod(type, model) {
        $(".edit").removeClass('edit');
        _self.$addToListItemModal.modal("hide");

        $.post(type == "group" ? '/todolist/update' : '/task/update',
            model,
            function (data, status) {
                if (type == "group")
                    getList();
                else
                    getTasks(_options.taskTodoId);
                _self.$addToListItemModal.modal("hide");
            }
        );
    }

    function clearModal() {
        _self.$itemName.val("");
        _self.$datetime.val("");
    }

    function eventListener() {
        $(document).on("change", "#groupList input", function () {
            marksAsDoneProccess("group", this);
        });

        $(document).on("change", "#list input", function () {
            _options.taskTodoId = $(this).parent().data("todoid");
            marksAsDoneProccess("list", this);
        });

        _self.$addItemGroup.on("click", function () {
            _self.$addToListItemModal.modal("show");
            _options.type = "group";
            _options.editType = false;
        });

        _self.$addItemList.on("click", function () {
            _self.$addToListItemModal.modal("show");
            _options.type = "list";
            _options.editType = false;
        });

        $(document).on("click", "#groupList li", function (e) {
            $('#groupList li').removeClass("selected");
            $(this).addClass("selected");
            _options.todoId = $(this).data("id");
            getTasks($(this).data("id"));
        });

        $(document).on("click", ".fa-trash-o", function () {
            var option = $(this).parents(".todo-list").attr('id') == 'groupList' ? 'group' : 'list';
            _options.type = option;

            deleteProccess($(this));
        });

        $(document).on("click", ".fa-edit", function () {
            _options.editType = true;
            var option = $(this).parents(".todo-list").attr('id') == 'groupList' ? 'group' : 'list';

            _options.type = option;
            _options.todoId = $(this).parent().parent().data("id");
            _options.taskTodoId = $(this).parent().parent().data("todoid");
            $(this).parent().parent().addClass('edit');

            editProccess(undefined, option);
            _self.$addToListItemModal.modal("show");
        });

        _self.$saveBtn.on("click", function () {
            //var option = $(this).parent().siblings(".box-body").find(".todo-list").attr('id') == 'groupList' ? 'group' : 'list';
            !_options.editType ? addItemProccess() : editProccess("post", _options.type);
        });
    }

    return init;

})(jQuery);


$(function () {
    TODOLIST.Module();
    $('.datetimepicker').datetimepicker({
        locale: 'tr'
    });
});
