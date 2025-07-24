(function () {
    function initPaginationGrid(options) {
		const {
			$el,
			url,
			columns,
			buttons = `<button type="button" class="btn btn-md btn-primary ml-2" id="btnCreate" style="min-width:150px">Create</button>`,
			getParams = () => ({}),
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

					return new URLSearchParams(params).toString();
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
			}
		});
	}

	function initPaginationDetailData(options) {
		const {
			$el,
			columns,
			buttons = `<button id="btnAddDetail" class="btn btn-sm btn-outline-primary mr-2 ml-2">Add Detail</button>`
		} = options;

		$el.dataTable({
			ordering: false,
			paging: false,
			info: false,
			searching: false,
			autoWidth: false,
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
			drawCallback: () => $el.find('select.select2').select2()
		})
	}

	function replaceData($el, obj) {
		$el.api().rows().remove();
		$el.api().rows.add(obj).draw();
	}

    window.DataTableHelper = {
		initPaginationGrid,
		initPaginationDetailData,
		replaceData
    };
})();
