(function () {
	function generateUUID() {
		return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
			(c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
		);
	}

	function getStartOfMonth(dt) {
		const firstDay = new Date(dt);
		firstDay.setDate(1);

		return firstDay;
	}

	function deleteData(url, grid) {
		Swal.fire({
			title: 'Delete',
			html: 'Apakah anda yakin untuk menghapus data ini?',
			width: '500px',
			showCancelButton: true,
			confirmButtonText: 'Delete',
			confirmButtonColor: "#D92D20",
			customClass: {
				actions: 'my-actions',
				cancelButton: 'order-1 right-gap',
				confirmButton: 'order-2',
			},
			icon: "warning",
			allowOutsideClick: () => !Swal.isLoading()
		}).then(async (result) => {
			if (result.isConfirmed) {
				const deleteResponse = await AjaxHelper.doAjax({
					method: "DELETE",
					url: url,
				});

				if (deleteResponse?.succeeded) {
					Swal.fire({
						title: "Success",
						text: deleteResponse.message,
						icon: "success"
					}).then(() => {
						grid.api().draw();
					});
				}
			}
		});
	}

	function deleteDataDetail(grid, rowIndex) {
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
		}).then((result) => {
			if (result.isConfirmed) {
				grid.api().row(rowIndex).remove().draw();
			}
		});
	}

	window.CommonHelper = {
		generateUUID,
		getStartOfMonth,
		deleteData,
		deleteDataDetail
	};
})();