.PHONY: generate
generate:
	python -m grpc_tools.protoc -I ../../proto --python_out=. --grpc_python_out=. ../../proto/improbable/spatialos/playerauth/v1alpha1/*.proto
	touch improbable/__init__.py
	touch improbable/spatialos/__init__.py
	touch improbable/spatialos/playerauth/__init__.py
	touch improbable/spatialos/playerauth/v1alpha1/__init__.py

.PHONY: install
install:
	pip install -r requirements.txt