rmdir Deploy\fluentworkflow /S/Q
rmdir Deploy\fluentworkflow.autofac /S/Q
md Deploy\fluentworkflow\lib\net40
md Deploy\fluentworkflow.autofac\lib\net40
copy core\fluentworkflow.core.dll deploy\fluentworkflow\lib\net40
copy core\fluentworkflow.core.testhelpers.dll deploy\fluentworkflow\lib\net40
copy core\Stateless.dll deploy\fluentworkflow\lib\net40
copy core\fluentworkflow.autofac.dll deploy\fluentworkflow.autofac\lib\net40
