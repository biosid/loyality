define('Catalog/Orders/export', [], function () {
    return function (options) {

        var startExportUrl = options.startUrl,
            exportBatchUrl = options.batchUrl,
            exportResultUrl = options.resultUrl,
            $exportBtn = $('[data-x="orders/export"]'),
            $exportBtnText = $exportBtn.find('span'),
            isInProgress = false,
            criteria,
            fileName,
            page;

        $exportBtn.on('click', startExport);
        
        function startExport() {
            if (isInProgress) {
                return;
            }

            startProgress();

            $.getJSON(startExportUrl)
                .done(function(data) {
                    criteria = data.criteria;
                    fileName = data.fileName;
                    page = 1;
                    startBatch();
                })
                .fail(exportFailed);
        }

        function startBatch() {
            $.getJSON(exportBatchUrl, { criteria: criteria, fileName: fileName, page: page })
                .done(batchDone)
                .fail(exportFailed);
        }

        function exportFailed() {
            alert('ошибка выгрузки заказов');
            resetProgress();
        }
        
        function batchDone(data) {
            if (!data || !data.status || data.status != 'ok') {
                exportFailed();
                return;
            }

            if (page == data.totalPages) {
                resetProgress();

                window.open(exportResultUrl + '?fileName=' + encodeURIComponent(fileName));
                return;
            }

            updateProgress(page / data.totalPages * 100);

            page = page + 1;
            startBatch();
        }
        
        function resetProgress() {
            isInProgress = false;
            $exportBtn.removeAttr('disabled');
            $exportBtnText.text('Выгрузить заказы');
        }
        
        function startProgress() {
            isInProgress = true;
            $exportBtn.attr('disabled', 'disabled');
            $exportBtnText.text('Идет выгрузка... 0%');
        }
        
        function updateProgress(progress) {
            $exportBtnText.text('Идет выгрузка... ' + Math.round(progress) + '%');
        }
    };
});