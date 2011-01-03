using System;
using System.Collections.Generic;
using System.Linq;
using fluentworkflow.core;
using sample.workflow.process.Workflow;

namespace sample.workflow.process.Services.Document
{
    public class InputProcessor
    {
        private readonly
            Func<DocumentType, WorkflowState,
                    IFluentStateEngine<DocumentType, WorkflowState, StateTrigger, IDocumentContext>> _stateMachineCreator;

        private readonly Func<int, IDocumentContext> _documentContextCreator;

        public InputProcessor(Func<DocumentType, WorkflowState, IFluentStateEngine<DocumentType, WorkflowState, StateTrigger, IDocumentContext>> stateMachineCreator,
                              Func<int, IDocumentContext> documentContextCreator)
        {
            _stateMachineCreator = stateMachineCreator;
            _documentContextCreator = documentContextCreator;
        }

        private StateTrigger? ProcessInputLoop(IEnumerable<KeyValuePair<StateTrigger, bool>> triggers)
        {
            while (true)
            {
                Console.Write("Enter Option: ");

                var option = Console.ReadLine();

                if (option == null) return null;

                if (option.ToUpper() == "Q")
                    return null;

                var selectedItems = triggers.Where(p => p.Key.ToString()[0].ToString() == option.ToString().ToUpper());

                if (selectedItems.Count() == 1)
                    return selectedItems.First().Key;
                
            }


        }

        public bool ProcessStateMove(int id)
        {
            var context = _documentContextCreator(id);

            var stateMachine = _stateMachineCreator(context.DocumentType, context.State);

            var items = stateMachine.GetTriggerOptions().Where(p => p.Value);

            Console.WriteLine();
            Console.WriteLine("******");
            Console.WriteLine("* Document State: {0}", stateMachine.State);
            Console.WriteLine("* User Name: {0}", context.UserName);
            Console.WriteLine("* Message: {0}", context.Message);
            Console.WriteLine("**");
            Console.WriteLine("* Menu Options");

            foreach(var item in items)
                Console.WriteLine("[{0}] - {1}", item.Key.ToString()[0], item.Key);

            Console.WriteLine("[Q] - Quit");
            Console.WriteLine();

            var trigger = ProcessInputLoop(items);

            if (trigger == null)
                return false;

            stateMachine.Fire(trigger.Value, context);

            return true;
        }
    }
}
