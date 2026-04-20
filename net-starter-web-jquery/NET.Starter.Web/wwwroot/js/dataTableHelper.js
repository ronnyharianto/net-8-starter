(function () {
    function initPaginationGrid(options) {
		const {
			$el,
			url,
			columns,
			buttons = `<button type="button" class="btn btn-md btn-primary ml-2" id="btnCreate" style="min-width:150px">Create</button>`,
			getParams = () => ({}),
			...opts
		} = options;

		$el.dataTable({
			ordering: false,
			serverSide: true,
			ajax: Object.assign({}, AjaxHelper.glbBaseRequest, {
				type: "GET",
				url,
				headers: { ...AjaxHelper.getDefaultHeaders() },
				data: (d) => {
					LoadingHelper.show();
					const params = getParams(d);
					const searchParams = new URLSearchParams();

					Object.entries(params).forEach(([key, value]) => {
						if (value === null || value === undefined || value === "") return;

						if (Array.isArray(value)) {
							value.forEach(v => {
								if (v !== null && v !== undefined && v !== "") {
									searchParams.append(key, v);
								}
							});
						} else {
							searchParams.append(key, value);
						}
					});

					return searchParams.toString();
				},
				dataSrc: (json) => {
					LoadingHelper.hide();

					return json.obj;
				}
			}),
			columns,
			layout: {
				topStart: null,
				topEnd: () => {
					const toolbar = document.createElement('div');
					if (buttons) {
						toolbar.innerHTML = buttons;
					}
					return toolbar;
				},
				bottomStart: 'info',
				bottomEnd: 'paging'
			},
			...opts
		});
	}

	function initPaginationDetailData(options) {
		const {
			$el,
			columns,
			scrollX = false,
			buttons = `<button type="button" id="btnAddDetail" class="btn btn-sm btn-outline-primary mr-2 ml-2">Add Detail</button>`,
			drawCallback = () => { },
			...opts
		} = options;

		$el.dataTable({
			ordering: false,
			paging: false,
			info: false,
			searching: false,
			autoWidth: false,
			scrollX: scrollX,
			columns,
			layout: {
				top1End: () => {
					const toolbar = document.createElement('div');
					if (buttons) {
						toolbar.innerHTML = buttons;
					}
					return toolbar;
				}
			},
			drawCallback: (settings) => {
				$el.find('select.select2').each(function () {
					if (!$(this).hasClass('select2-hidden-accessible')) {
						$(this).select2({
							dropdownParent: $el.closest('.dt-container')
						});
					}
				});

				const $dtPickers = $el.find('.datepicker');
				$dtPickers.datepicker({
					dateFormat: "yy-mm-dd"
				});

				if (drawCallback && typeof drawCallback === 'function')
					drawCallback.call(this, settings);
			},
			...opts
		})
	}

	function clearAjaxDataTable($el, totalColumn) {
		$el.find('tbody')
		   .empty()
		   .append(`<tr><td colspan="${totalColumn}" class="dataTables_empty text-center">No matching records found</td></tr>`);
	}

	function clearData($el, doDraw = true) {
		$el.api().rows().clear();

		if (doDraw)
            $el.api().draw();
    }

	function replaceData($el, obj, doDraw = true) {
		clearData($el, false);
		$el.api().rows.add(obj);

        if (doDraw)
            $el.api().draw();
	}

	function createRow($el, rowIndex, obj, doDraw = true) {
		const data = $el.api().data().toArray();

		data.splice(rowIndex, 0, obj);

		$el.api().rows().clear();
		$el.api().rows.add(data);

        if (doDraw)
            $el.api().draw();
	}

	function updateRow($el, rowIndex, prop, value, doDraw = true) {
		const rowData = $el.api().row(rowIndex).data();
		rowData[prop] = value;

		if (doDraw) {
			$el.api().row(rowIndex).data(rowData);
			$el.api().draw();
		}
	}

	function deleteRow($el, rowIndex, doDraw = true) {
		Swal.fire({
			title: "Hapus Data",
			html: "Apakah anda yakin ingin meghapus data ini?",
			showCancelButton: true,
			confirmButtonText: "Hapus",
			confirmButtonColor: "#D92D20",
			customClass: {
				actions: 'my-actions',
				cancelButton: 'order-1 right-gap',
				confirmButton: 'order-2',
			},
			icon: "warning"
		})
		.then((result) => {
			if (result.isConfirmed) {
				$el.api().row(rowIndex).remove();

                if (doDraw)
                    $el.api().draw();
			}
		});
    }

    window.DataTableHelper = {
		initPaginationGrid,
		initPaginationDetailData,
		clearAjaxDataTable,
		clearData,
		replaceData,
		createRow,
		updateRow,
		deleteRow
    };
})();
