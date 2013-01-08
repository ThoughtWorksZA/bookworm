$(document).ready(function () {
    $('input[data-confirm]').click(function () {
        var self = this;
        if (!$('#dataConfirmModal').length) {
            $('body').append('<div id="dataConfirmModal" class="modal hide fade" role="dialog" aria-labelledby="dataConfirmLabel" aria-hidden="true"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button><h3 id="dataConfirmLabel">Are you sure?</h3></div><div class="modal-body"></div><div class="modal-footer"><button class="btn btn-primary" id="dataConfirmOK">OK</button><button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button></div></div>');
        }
        $('#dataConfirmModal').find('.modal-body').text($(this).attr('data-confirm'));
        $('#dataConfirmOK').click(function () { self.form.submit(); });
        $('#dataConfirmModal').modal({ show: true });
        return false;
    });
});