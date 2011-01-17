rmdir Deploy\lib /S/Q
md Deploy\lib
md Deploy\lib\net40
copy core\fluentworkflow.core.dll deploy\lib\net40
copy core\fluentworkflow.core.testhelpers.dll deploy\lib\net40
copy core\Stateless.dll deploy\lib\net40
copy core\AutofacContrib.Attributed.dll deploy\lib\net40